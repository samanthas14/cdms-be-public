using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;

/*
 * DecimalPrecisionAttribute allows you to annotate a decimal property of an entity with the precision desired:
 * 
 *  [DecimalPrecision(20,10)]
 *  public Nullable<decimal> DeliveryPrice { get; set; }
 * 
 * credit: https://stackoverflow.com/questions/3504660/decimal-precision-and-scale-in-ef-code-first/15386883#15386883
 * 
 * the DecimalPrecisionAttributeConvention can then be referenced in our ServicesContext
 */

namespace services.Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DecimalPrecisionAttribute : Attribute
    {
        public DecimalPrecisionAttribute(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;
        }
        public byte Precision { get; set; }
        public byte Scale { get; set; }

    }

    public class DecimalPrecisionAttributeConvention
    : PrimitivePropertyAttributeConfigurationConvention<DecimalPrecisionAttribute>
    {
        public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DecimalPrecisionAttribute attribute)
        {
            if (attribute.Precision < 1 || attribute.Precision > 38)
            {
                throw new InvalidOperationException("Precision must be between 1 and 38.");
            }

            if (attribute.Scale > attribute.Precision)
            {
                throw new InvalidOperationException("Scale must be between 0 and the Precision value.");
            }

            configuration.HasPrecision(attribute.Precision, attribute.Scale);
        }
    }
}

