using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Demos.Dynamic
{
    public class MultiLevelExpando : DynamicObject
    {
        private readonly string _name;
        private object _value;
        private bool _isMidLevel;
        private readonly IDictionary<string, MultiLevelExpando> _attributes = new Dictionary<string, MultiLevelExpando>();

        public MultiLevelExpando(string name)
        {
            _name = name;
        }

        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            if (binder.Operation != ExpressionType.OnesComplement)
            {
                return base.TryUnaryOperation(binder, out result);
            }

            result = get_xml_value();
            return true;  
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_attributes.ContainsKey(binder.Name))
            {
                _attributes[binder.Name]._value = value;
            }
            else
            {
                (_attributes[binder.Name] = new MultiLevelExpando(binder.Name))._value = value;
            }
            
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_attributes.ContainsKey(binder.Name))
            {
                var element = _attributes[binder.Name];
                result = element._isMidLevel ? element : element._value;
            }
            else
            {
                result = _attributes[binder.Name] = new MultiLevelExpando(binder.Name) { _isMidLevel = true };
            }

            return true;
        }

        private object get_xml_value()
        {
            if (_attributes.Count == 0)
            {
                return new XAttribute(_name, _value);
            }

            return new XElement(_name, _attributes.Values.Select(x => x.get_xml_value()), _value ?? "");
        }

        [Ignore]
        public static void Run()
        {
            dynamic person = new MultiLevelExpando("Person");
            person.Name = "Some Name";
            person.Address.StreetNumber = 8;
            person.Address.StreetName = "Some Street";

            XElement elem = ~person;
            Console.WriteLine(elem);
        }
    }
}
