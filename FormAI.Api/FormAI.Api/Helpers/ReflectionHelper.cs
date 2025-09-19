using System.Reflection;

namespace FormAI.Api.Helpers
{
    public class ReflectionHelper
    {
        public static T CopyObject<T>(object source)
        {
            var destination = Activator.CreateInstance<T>();
            CopyProperties(source, destination);
            return destination;
        }

        public static double GetPropertyValue(object instance, string propertyName)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            var value = pi.GetValue(instance, null);
            return Convert.ToDouble(value);
        }

        public static void CopyProperties(object source, object destination)
        {
            // Verifica se a origem e o destino são diferentes de null
            if (source == null || destination == null)
                throw new ArgumentNullException("Source or/and Destination Objects are null");

            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Obtém as propriedades do objeto de origem
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (var srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                // Encontra a propriedade correspondente no objeto de destino
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPublic)
                {
                    //// Verifica se o tipo de propriedade é o mesmo
                    //if (targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                    //{
                    // Copia o valor da propriedade
                    targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
                    //}
                }
            }
        }
    }
}
