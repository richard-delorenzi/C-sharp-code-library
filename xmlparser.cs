using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
//using Richard.Contracts;
using System.Diagnostics.Contracts;

namespace Richard
{
    class XmlParser : XmlParser<string> { }
    class XmlParser<OutputBaseType> : XmlParser<OutputBaseType, List<OutputBaseType>>
    {}
    class XmlParser<OutputBaseType,OutputType> 
        where OutputType : ICollection<OutputBaseType>, new()
    {
        public static OutputBaseType OutputBaseTypeVoid = default(OutputBaseType);
        public enum Errors { None, InvalidXml };
        public delegate OutputBaseType xmlDelegate(Dictionary<string, string> dict, string value);

        //Constructor
        public XmlParser() { }

        public void Add(
            IEnumerable<string> stack,
            Dictionary<string, string> SharedDictionary,
            xmlDelegate delegate_elementText,
            xmlDelegate delegate_elementEnter,
            xmlDelegate delegate_elementExit
            // :tricky: you are expected to break command query separation with these delegates
            // :tricky: delegates can return null
            )
        {
            _dict.Add(
                new Stack<string>(stack),
                new Actions(SharedDictionary, delegate_elementText, delegate_elementEnter, delegate_elementExit)
                );
        }

        public Errors Error { get; private set; }

        public OutputType Output
        {
            get
            {
                Contract.Requires(Error == Errors.None);
                return _Output;
            }
        }

        public void Process(string Filename)
        {
            Contract.Requires(Filename != null);
            try
            {
                _Process(Filename);
            }
            catch (System.Xml.XmlException)
            {
                Error = Errors.InvalidXml;
            }
        }

        #region private

        OutputType _Output;
        Dictionary<Stack<string>, Actions> _dict = new Dictionary<Stack<string>, Actions>();

        private class Actions
        {
            public Actions(
                Dictionary<string, string> SharedDictionary,
                xmlDelegate ElementTextAction,
                xmlDelegate ElementEnterAction,
                xmlDelegate ElementExitAction)
            {
                this.SharedDictionary = SharedDictionary;
                this.ElementTextAction = ElementTextAction;
                this.ElementEnterAction = ElementEnterAction;
                this.ElementExitAction = ElementExitAction;
            }

            public Dictionary<string, string> SharedDictionary;
            public xmlDelegate ElementTextAction;
            public xmlDelegate ElementEnterAction;
            public xmlDelegate ElementExitAction;
        }

        private XmlReader Create(string filename, XmlReaderSettings settings)
        {
            Contract.Requires(filename != null);

            while (true) //until successful
            {
                try
                {
                    return XmlReader.Create(filename, settings);
                }
                catch (System.IO.IOException ex) //:todo: hack
                {
                    if (ex.Message.Contains("because it is being used by another process"))
                    {
                        System.Threading.Thread.Sleep(10);
                        //retry
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void _Process(string Filename)
        {
            Contract.Requires(Filename != null);

            var settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            var stack = new System.Collections.Generic.Stack<string>();
            _Output = new OutputType();
            Error = Errors.None;

            using (var reader = Create(Filename, settings)) while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        _ProcessElement(reader, stack);
                        break;
                    case XmlNodeType.Text:
                        _ProcessText(reader, stack);
                        break;
                    case XmlNodeType.EndElement:
                        _ProcessEndElement(reader, stack);
                        break;
                }
            }//end using
        }//end 

        private void _ProcessElement(
            XmlReader reader,
            System.Collections.Generic.Stack<string> stack)
        {
            Contract.Requires(reader != null);
            Contract.Requires(stack != null);
            Contract.Ensures(stack.Count <= Contract.OldValue(stack.Count + 1));
            Contract.Ensures(stack.Count >= Contract.OldValue(stack.Count ));

            stack.Push(reader.Name);
            _LookForMatch(reader.Value, stack, (action) => action.ElementEnterAction);

            _ProcessAttributes(reader, stack);

            if (reader.IsEmptyElement)
            {
                _ProcessEndElement(reader, stack);
            }
        }

        private void _ProcessAttributes(
           XmlReader reader,
           System.Collections.Generic.Stack<string> stack)
        {
            Contract.Requires(reader != null);
            Contract.Requires(stack != null);
            Contract.Requires(Contract.ForAll(stack, (x) => (x != null)));
            Contract.Ensures(stack.Count == Contract.OldValue(stack.Count));

            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    stack.Push("$" + reader.Name);
                    _LookForMatch(reader.Value, stack, (action) => action.ElementTextAction);
                    stack.Pop();
                }
                // Move the reader back to the element node.
                reader.MoveToElement();
            }
        }

        private void _ProcessText(
           XmlReader reader,
           System.Collections.Generic.Stack<string> stack)
        {
            Contract.Requires(reader != null);
            Contract.Requires(stack != null);
            Contract.Requires(Contract.ForAll(stack, (x) => (x != null)));
            Contract.Ensures(stack.Count == Contract.OldValue(stack.Count));

            _LookForMatch(reader.Value, stack, (action) => action.ElementTextAction);
        }

        private void _ProcessEndElement(
           XmlReader reader,
           System.Collections.Generic.Stack<string> stack)
        {
            Contract.Requires(reader != null);
            Contract.Requires(stack != null);
            Contract.Requires(Contract.ForAll(stack, (x) => (x != null)));
            Contract.Ensures(stack.Count <= Contract.OldValue(stack.Count));
            Contract.Ensures(stack.Count >= Contract.OldValue(stack.Count-1));
            Contract.Ensures((Error == Errors.None).Implies(
                stack.Count == Contract.OldValue(stack.Count - 1)));

            _LookForMatch(reader.Value, stack, (action) => action.ElementExitAction);

            if (reader.Name == stack.Peek())
            {
                stack.Pop();
            }
            else
            {
                Error = Errors.InvalidXml;
            }
        }

        private delegate xmlDelegate ActionTypeDelegate(Actions a);

        private void _LookForMatch(
            string value,
            System.Collections.Generic.Stack<string> stack,
            ActionTypeDelegate ActionType)
        {
            Contract.Requires(value != null);
            Contract.Requires(stack != null);
            Contract.Requires(ActionType != null);
            Contract.Requires(Contract.ForAll(stack, (x) => (x != null )));
            Contract.Ensures(stack.Count == Contract.OldValue(stack.Count));

            foreach (var attempt in _dict) if (attempt.Key.SequenceEqual(stack))
            {
                var action = ActionType(attempt.Value);
                OutputBaseType outputItem = action(attempt.Value.SharedDictionary, value);
                if ( !EqualityComparer<OutputBaseType>.Default.Equals(
                    outputItem, 
                    default(OutputBaseType))) 
                {
                    _Output.Add(outputItem); 
                }
            }
        }
        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant () 
        {
            Contract.Invariant ( _dict != null );
            Contract.Invariant(Contract.ForAll(_dict, (x) => (x.Key != null) && (x.Value != null) ));
            
        }
    }
}
