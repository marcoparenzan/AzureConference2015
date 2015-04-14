using Call4Pizza.Models.Commands;
using Call4Pizza.Models.DTO;
using Call4Pizza.Models.Entities;
using Call4Pizza.Models.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Call4Pizza.Models.AggregateRoots
{
    public class QueryingOrder : Call4PizzaAggregateRoot<CommandingOrder, Guid>, IAggregateRootSerializable
    {
        protected List<OrderPizza> _pizzas;
        protected List<OrderBeverage> _beverages;

        protected int _seatings;
        protected Guid _commandId;

        public QueryingOrder()
        {
            _pizzas = new List<OrderPizza>();
            _beverages = new List<OrderBeverage>();
        }

        protected override Guid OnGetId()
        {
            return _commandId;
        }

        void IAggregateRootSerializable.SerializeTo(Stream stream)
        {
             var serializer = new JsonSerializer();
             serializer.Converters.Add(new JavaScriptDateTimeConverter());
             serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter(stream))
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("CommandId"); writer.WriteValue(_commandId);
                    writer.WritePropertyName("Seatings"); writer.WriteValue(_seatings);
                    writer.WritePropertyName("Pizzas"); serializer.Serialize(writer, _pizzas);
                    writer.WritePropertyName("Beverages"); serializer.Serialize(writer, _beverages);
                    writer.WriteEndObject();
                }
            }
        }

        void IAggregateRootSerializable.DeserializeFrom(System.IO.Stream stream)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sr = new StreamReader(stream))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    while(reader.TokenType != JsonToken.StartObject)  reader.Read();
                    reader.Read();
                    while (true)
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                        {
                            reader.Read();
                            break;
                        }

                        switch (reader.TokenType)
                        {
                            case JsonToken.PropertyName:
                                DeserializeProperty(serializer, reader, reader.Value as string);
                                break;
                            case JsonToken.Comment:
                                reader.Read();
                                break;
                            default:
                                throw new NotSupportedException("Token not recognized");
                        }
                    }
                }
            }
        }

        private void DeserializeProperty(JsonSerializer serializer, JsonTextReader reader, string propertyName)
        {
            switch (propertyName)
            {
                case "CommandId":
                    _commandId = Guid.Parse(reader.ReadAsString());reader.Read();
                    break;
                case "Seatings":
                    _seatings = reader.ReadAsInt32() ?? 0;reader.Read();
                    break;
                case "Pizzas":
                    reader.Read();
                    _pizzas = serializer.Deserialize<List<OrderPizza>>(reader);reader.Read();
                    break;
                case "Beverages":
                    reader.Read();
                    _beverages = serializer.Deserialize<List<OrderBeverage>>(reader);reader.Read();
                    break;
                default:
                    throw new NotSupportedException("Property not recognized");
            }
        }

        public IEnumerable<PizzaToDoDTO> PizzasToDo()
        {
            return _pizzas.Select(xx => new PizzaToDoDTO { 
                Description = xx.Description
                ,
                Quantity = xx.Quantity
                ,
                CommandId = _commandId
            }).ToList();
        }
    }
}
