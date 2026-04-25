using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Shared
{
    public static class PrimitiveExtensions
    {
        public static string CulturaAmbysoft = "en-US";
        public static int DecimalesAmbysoft = 2;


        public static string ToJson(this object? objeto)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Reuse,
                NullValueHandling = NullValueHandling.Ignore,
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(objeto, settings);
        }

        public static string ToXml<T>(this T? objeto)
        {
            if (objeto == null)
            {
                return string.Empty;
            }
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty); // Remueve los espacios de nombres

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true, // Omite la declaración XML
                Indent = true // Indentación opcional, para legibilidad
            };

            using (StringWriter writer = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    serializer.Serialize(xmlWriter, objeto, namespaces);
                    return writer.ToString();
                }
            }
        }

        public static T? FromJson<T>(this string? objeto)
        {
            return objeto == null ? default : JsonConvert.DeserializeObject<T>(objeto);
        }

        public static decimal ConvertObjectToDecimal(this object objeto)
        {
            string datoObjeto = objeto == null || objeto.ToString() == "" ? "0" : objeto.ToString()!;

            datoObjeto = System.Text.RegularExpressions.Regex
        .Replace(datoObjeto, @"\s*USD\s*$", "")
        .Trim();

            char[] sep = datoObjeto.Where(x => char.IsPunctuation(x) && x != '-').Reverse().Append('$').Append('€').ToArray();
            var seps = sep.Count() - 1;
            while (seps > 0)
            {
                datoObjeto = datoObjeto.Replace(sep[seps].ToString(), "");
                seps--;
            }
            return decimal.Parse(datoObjeto!
                    .Replace(sep[0].ToString(), CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
        }

        public static bool TryConvertObjectToDecimal(this object objeto, out decimal returnValue)
        {
            return decimal.TryParse(objeto.ToString()!
                    .Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), out returnValue);
        }

        public static decimal? ReplaceDecimalIfNull(this object objeto, decimal? newValueIfNullEmpty)
        {
            bool value = false;
            decimal valueOut = 0;
            if (objeto != null)
                value = decimal.TryParse(objeto.ToString()!
                    .Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), out valueOut);
            if (!value) return newValueIfNullEmpty;
            return valueOut;
        }


        public static int ConvertObjectToInt(this object objeto)
        {
            var decValue = objeto.ConvertObjectToDecimal();
            return int.Parse(decValue.ToString()[..^(decValue.Scale > 0 ? decValue.Scale + 1 : 0)]!);
        }

        public static int Abs(this int valor)
        {
            return Math.Abs(valor);
        }

        public static bool TryConvertObjectToInt(this object objeto, out int returnValue)
        {
            var decValue = objeto.ConvertObjectToDecimal();
            return int.TryParse(decValue.ToString()[..^(decValue.Scale > 0 ? decValue.Scale + 1 : 0)]!, out returnValue);
        }

        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value, params char[] startEndomitted)
        {
            string texto = value.ReplaceIfNullOrEmpty().Trim(startEndomitted);
            return texto.IsNullOrEmpty();
        }

        public static string ReplaceIfNullOrEmpty(this string? value)
        {
            return value.ReplaceIfNullOrEmpty(string.Empty);
        }

        public static string ReplaceIfNullOrEmpty(this string? value, string newValueIfNullEmpty)
        {
            if (value.IsNullOrEmpty()) return newValueIfNullEmpty;
            return value!;
        }

        public static string ReplaceIfNullOrEmpty(this object obj, string newValueIfNullEmpty)
        {
            string? value = null;
            if (obj != null)
                value = obj.ToString()!;
            if (string.IsNullOrEmpty(value)) return newValueIfNullEmpty;
            return value;
        }

        public static string ReplaceBoleanString(this string? value)
        {
            if (value != null && value.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                return "SI";
            if (value != null && value.Equals("false", StringComparison.CurrentCultureIgnoreCase))
                return "NO";
            return value.ReplaceIfNullOrEmpty();
        }

        public static string ReplaceBoleanString(this bool value)
        {
            if (value)
                return "SI";
            return "NO";
        }

        public static string ReplaceBoleanString(this bool? value)
        {
            if (value != null && value.Equals(true))
                return "SI";
            return "NO";
        }


        public static string SureStartWith(this string texto, char startValue)
        {
            return texto.SureStartWith(startValue.ToString());
        }

        public static string SureStartWith(this string texto, string startValue)
        {
            if (texto.StartsWith(startValue, StringComparison.InvariantCultureIgnoreCase))
                return texto;
            else
                return $"{startValue}{texto}";
        }

        public static string SureEndsWith(this string texto, char startValue)
        {
            return texto.SureEndsWith(startValue.ToString());
        }

        public static string SureEndsWith(this string texto, string startValue)
        {
            if (texto.EndsWith(startValue, StringComparison.InvariantCultureIgnoreCase))
                return texto;
            else
                return $"{texto}{startValue}";
        }
        public static string EnclosedIn(this string texto, char startValue)
        {
            return texto.EnclosedIn(startValue, startValue);
        }

        public static string EnclosedIn(this string texto, char startValue, char endValue)
        {
            return texto.EnclosedIn(startValue.ToString(), endValue.ToString());
        }

        public static string EnclosedIn(this string texto, string startValue)
        {
            return texto.EnclosedIn(startValue, startValue);
        }

        public static string EnclosedIn(this string texto, string startValue, string endValue)
        {
            return texto.SureStartWith(startValue).SureEndsWith(endValue);
        }

        public static string ToTitle(this string value)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ReplaceIfNullOrEmpty().Trim().ToLower());
        }

        public static bool IsEmail([NotNullWhen(true)] this string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email.Trim(), pattern);
        }

        public static string ToBase64(this int value)
        {
            return value.ToBase64(Encoding.UTF8);
        }

        public static string ToBase64(this int value, Encoding encoding)
        {
            return ToBase64(value.ToString(), encoding);
        }

        public static string ToBase64(this decimal value)
        {
            return value.ToBase64(Encoding.UTF8);
        }

        public static string ToBase64(this decimal value, Encoding encoding)
        {
            return ToBase64(value.ToString(), encoding);
        }

        public static string ToBase64(this DateTime value)
        {
            return value.ToBase64(Encoding.UTF8);
        }

        public static string ToBase64(this DateTime value, Encoding encoding)
        {
            return ToBase64(value.ToString(), encoding);
        }

        public static string ToBase64(this string value)
        {
            return value.ToBase64(Encoding.UTF8);
        }

        public static string ToBase64(this string value, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(value));
        }

        public static void AddIfNotExists<TK, Tv>(this IDictionary<TK, Tv> dic, TK key, Tv value)
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, value);
        }

        public static void AddOrUpdate<TK, Tv>(this IDictionary<TK, Tv> dic, TK key, Tv value)
        {
            if (dic.ContainsKey(key))
                dic[key] = value;
            else
                dic.Add(key, value);
        }

        public static bool Between(this DateTime value, DateTime ini, DateTime fin)
        {
            return value >= ini && value <= fin;
        }

        public static DateTime TruncYear(this DateTime value)
        {
            return new DateTime(value.Year, 1, 1);
        }

        public static DateTime TruncMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static DateTime TruncHour(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0);
        }

        public static DateTime TruncMinute(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0);
        }

        public static DateTime ConvertObjectToDate(this object objeto, bool includeTime = false, bool useMMDDYYYYFormat = false)
        {
            string format;

            if (includeTime)
            {
                format = useMMDDYYYYFormat ? "M/d/yyyy HH:mm:ss" : "yyyy-M-d HH:mm:ss";
            }
            else
            {
                format = useMMDDYYYYFormat ? "M/d/yyyy" : "d/M/yyyy";
            }

            string fechaCadena = (objeto?.ToString()).ReplaceIfNullOrEmpty();
            if (!includeTime && fechaCadena.Contains(':'))
                fechaCadena = fechaCadena[..fechaCadena.IndexOf(' ')];

            if (DateTime.TryParseExact(fechaCadena, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaCelda))
            {
                return fechaCelda;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static int Max(this int a, int b)
        {
            return a > b ? a : b;
        }

        public static decimal Max(this decimal a, decimal b)
        {
            return a > b ? a : b;
        }

        public static double Max(this double a, double b)
        {
            return a > b ? a : b;
        }

        public static DateTime? Max(this DateTime? a, DateTime? b)
        {
            if (a == null || b == null)
                return null;
            if (a != null || b == null)
                return a;
            if (a == null || b != null)
                return b;

            return a > b ? a : b;
        }

        public static int Min(this int a, int b)
        {
            return a < b ? a : b;
        }

        public static decimal Min(this decimal a, decimal b)
        {
            return a < b ? a : b;
        }

        public static double Min(this double a, double b)
        {
            return a < b ? a : b;
        }

        public static DateTime Min(this DateTime a, DateTime b)
        {
            return a < b ? a : b;
        }

        public static string DescriptionEnum(this Enum valorEnum)
        {
            FieldInfo campo = valorEnum.GetType().GetField(valorEnum.ToString())!;
            if (campo != null)
            {
                DescriptionAttribute atributo =
                    (DescriptionAttribute)Attribute.GetCustomAttribute(campo, typeof(DescriptionAttribute))!;

                if (atributo != null)
                {
                    return atributo.Description;
                }
            }
            return valorEnum.ToString();
        }

        public static T GetEnumByName<T>(this string valorEnum) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), valorEnum);
        }

        public static DateTime StartMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static DateTime EndMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month)).AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public static DateTime EndDay(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
        }
        public static DateTime StartDay(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 00, 00, 00);
        }

        public static DateTime DateTimeParseExact(this string value, string formato)
        {
            return DateTime.ParseExact(value, formato, null, System.Globalization.DateTimeStyles.None);
        }

        public static string Trunc(this string texto, int caracteres)
        {
            return texto.IsNullOrEmpty() || texto.Length < caracteres ? texto : texto[0..caracteres];
        }

        public static string[] TruncWords(this string texto, int caracteres)
        {
            var componentes = texto.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x + " ");
            List<string> result = new();

            int idx = 0;
            foreach (var item in componentes)
            {
                string dato = string.Empty;
                if (result.Count > 0)
                    dato = result[idx];

                if ((dato + item).Length > caracteres)
                {
                    idx++;
                    result.Add(item);
                }
                else
                {
                    if (result.Count > 0)
                        result.RemoveAt(idx);
                    result.Insert(idx, (dato + item));
                }
            }
            return result.Select(x => x.Trim()).ToArray();
        }


        public static string CalculateMD5Hash(this string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                return inputBytes.CalculateMD5Hash();
            }
        }

        public static string CalculateMD5Hash(this byte[] inputBytes)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        // Método para ordenar por un campo específico utilizando reflexión
        public static IQueryable<T> OrderByName<T>(this IQueryable<T> q, string campo, bool ascendente = true) where T : class
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, campo);
            var exp = Expression.Lambda(prop, param);
            string method = ascendente ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
        public static IQueryable<T> ThenOrderByName<T>(this IQueryable<T> q, string campo, bool ascendente = true) where T : class
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, campo);
            var exp = Expression.Lambda(prop, param);
            string method = ascendente ? "ThenBy" : "ThenByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }

        public static IQueryable<T> WhereAny<T, V>(this IQueryable<T> query, IEnumerable<V> listaDeStrings, Expression<Func<T, V, bool>> funcionComparacion)
        {
            var parameter = funcionComparacion.Parameters[0];
            var body = listaDeStrings
                .Select(value => Expression.Constant(value, typeof(V)))
                .Select(value => Expression.Invoke(funcionComparacion, parameter, value))
                .Aggregate<Expression>(Expression.OrElse);
            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
            return query.Where(predicate);
        }

        public static IQueryable<T> WhereAny<T, V>(this IEnumerable<T> query, IEnumerable<V> listaDeStrings, Expression<Func<T, V, bool>> funcionComparacion)
        {
            var parameter = funcionComparacion.Parameters[0];
            var body = listaDeStrings
                .Select(value => Expression.Constant(value, typeof(V)))
                .Select(value => Expression.Invoke(funcionComparacion, parameter, value))
                .Aggregate<Expression>(Expression.OrElse);
            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
            return query.AsQueryable().Where(predicate);
        }

        public static DateTime FromUtc(this DateTime fecha)
        {
            // Convertir la fecha a la zona horaria GMT-5
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(fecha, "America/Guayaquil");
        }

        public static string GetPropertyValue(this object obj, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return string.Empty;

            Type type = obj.GetType();
            PropertyInfo? property = type.GetProperty(propertyName.Trim('{', '}', '#'), BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                return string.Empty;

            object? value = property.GetValue(obj);
            return value?.ToString() ?? string.Empty;
        }

        public static IEnumerable<object>? GetArrayPropertyValue(this object obj, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return Array.Empty<object>();

            Type type = obj.GetType();
            PropertyInfo? property = type.GetProperty(propertyName.Trim('{', '}', '#'));
            return property != null ? property.GetValue(obj) as IEnumerable<object> : Array.Empty<object>();
        }

        public static int ExtractNumbers(this string input)
        {
            // Definir una expresión regular para encontrar números
            Regex regex = new Regex(@"\d+");
            MatchCollection matches = regex.Matches(input);

            // Concatenar todos los números encontrados
            string numbers = string.Empty;
            foreach (Match match in matches)
            {
                numbers += match.Value;
            }
            return int.Parse(numbers);
        }

        public static bool IsValidIdentificationEcuador(this string cedula, out string? cedulaValida)
        {
            cedulaValida = string.Join("", cedula.ReplaceIfNullOrEmpty().Where(c => char.IsDigit(c)).Select(c => c));
            if (cedulaValida.Length != 10)
                return false;

            int provincia = int.Parse(cedulaValida.Substring(0, 2));
            if (provincia < 1 || provincia > 24 && provincia != 30)
                return false;

            int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int suma = 0;

            for (int i = 0; i < coeficientes.Length; i++)
            {
                int digito = int.Parse(cedulaValida.Substring(i, 1));
                int producto = digito * coeficientes[i];
                suma += producto >= 10 ? producto - 9 : producto;
            }

            int ultimoDigito = int.Parse(cedulaValida.Substring(9, 1));
            int digitoVerificador = 10 - (suma % 10);
            digitoVerificador = digitoVerificador == 10 ? 0 : digitoVerificador;

            bool resultado = ultimoDigito == digitoVerificador;
            return resultado;
        }

        public static bool IsValidRucEcuador(this string ruc, out string? rucValido)
        {
            rucValido = string.Join("", ruc.ReplaceIfNullOrEmpty().Where(c => char.IsDigit(c)).Select(c => c));

            if (rucValido.Length != 13)
                return false;

            int provincia = int.Parse(rucValido[..2]);
            if (provincia < 1 || provincia > 24 && provincia != 30)
                return false;

            if (rucValido.Substring(10, 2) != "00" && rucValido[12].ConvertObjectToInt() > 0)
                return false;

            return true;
        }

        public static bool IsValidMail(this string correo, out string correoValido)
        {
            correoValido = correo.ReplaceIfNullOrEmpty().Trim().ToLower();
            if (!correo.IsNullOrEmpty())
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);
                return regex.IsMatch(correo);
            }
            return false;
        }

        public static bool IsValidCelularPhoneEcuador(this string telefono, out string telefonoValido)
        {
            telefonoValido = string.Join("", telefono.ReplaceIfNullOrEmpty().Where(c => char.IsDigit(c)).Select(c => c));

            if (telefonoValido.StartsWith("593"))
                telefonoValido = string.Concat("0", telefonoValido.AsSpan(3));

            telefono = telefono.SureStartWith('0');

            return telefonoValido.Length == 10 && telefonoValido.StartsWith("09");
        }

        public static bool IsValidPassport(this string pasaporte, out string pasaporteValido)
        {
            pasaporteValido = pasaporte.ReplaceIfNullOrEmpty().Trim().ToUpper();
            string patron = @"^[A-Z]{1,3}[0-9]{6,10}$";
            Regex regex = new Regex(patron);
            return true;
            //regex.IsMatch(pasaporteValido);
        }

        /// <summary>
        /// Obtiene cédula, NombreCompleto,Correo,Telefono por el número de cedula o telefono.
        /// </summary>
        /// <param name="valorTotal">Valor del producto</param>
        /// <paramref name="tasaAnual">Tasa de interes anual</paramref>
        /// <param name="plazoMeses">Numero de Meses/param>
        /// <param name="valorEntrada">Valor de Entrada</param>
        /// <paramref name="componentesAdicionales">Valores extras</paramref>
        /// <returns>Un decimal con la cuota mensual</returns>
        public static decimal ValorCuota(this decimal valorTotal, decimal tasaAnual, int plazoMeses, decimal valorEntrada = 0M, decimal porcentajeSeguroDesgravamen = 0M, decimal componentesAdicionales = 0M)
        {
            if (valorEntrada <= 0 && valorEntrada > valorTotal)
                throw new Exception("Revisar el valor de entrada.");
            var capital = valorTotal + componentesAdicionales - valorEntrada;
            decimal tasaMensual = tasaAnual / 100M / 12M;
            decimal factor = (decimal)Math.Pow((double)(1 + tasaMensual), plazoMeses);
            decimal seguroDesgravamen = capital * porcentajeSeguroDesgravamen / 36000M * 30M;
            return 0M;
            //return Math.Round(((capital * tasaMensual * factor) / (factor - 1)) + seguroDesgravamen, 2);
        }

        public static string CapitalizarString(this string? cadena)
        {
            if (string.IsNullOrEmpty(cadena))
            {
                return string.Empty;
            }
            cadena = cadena.Trim().ToLower();

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cadena);
        }

        public static NombreApellidoSeparado SepararNombresApellidos(this string nombreUnido, bool empiezaConApellido)
        {
            var nombreCompleto = string.Format(" {0} ", string.Join(" ", nombreUnido.Split(new char[] { ' ', '.', ',', '#', '-', '@', ':', '~' })));
            var listaNombres = new List<string>();
            var listaApellidos = new List<string>();

            #region Conjuntos de apellidos y nombres

            var listaConjunto = new List<string>
                        {
                            " DE LAS ",
                            " DE LA ",
                            " DE LOS ",
                            " DE OCA ",
                            " DE ",
                            " LAS ",
                            " LA ",
                            " DEL ",
                        };

            #endregion

            #region Remplazo de conjuntos

            for (int i = 0; i < listaConjunto.Count; i++)
                nombreCompleto = nombreCompleto.Replace(listaConjunto[i], string.Format(" #{0}", i));

            #endregion

            #region Separar

            var partes = nombreCompleto.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var parNombres = 2;
            var parApellidos = 0;

            if (partes.Length < 2 && empiezaConApellido)
            {
                parApellidos = partes.Length;
                parNombres = 0;
            }
            else if (partes.Length < 2)
                parNombres = partes.Length;
            else if (partes.Length == 2)
            {
                parApellidos = 1;
                parNombres = 1;
            }
            else if (partes.Length == 3 && empiezaConApellido)
            {
                parApellidos = 2;
                parNombres = 1;
            }
            else if (partes.Length == 3)
                parApellidos = 1;
            else if (partes.Length == 4)
                parApellidos = 2;
            else
            {
                var medio = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(partes.Length) / 2M));
                parApellidos = empiezaConApellido ? partes.Length - medio : medio;
                parNombres = empiezaConApellido ? partes.Length - medio : medio;
            }

            for (int i = 0; i < partes.Length; i++)
            {
                if (empiezaConApellido && parApellidos > 0)
                {
                    listaApellidos.Add(partes[i]);
                    parApellidos--;
                }
                else if (parNombres > 0)
                {
                    listaNombres.Add(partes[i]);
                    parNombres--;
                    if (parNombres == 0) empiezaConApellido = true;
                }
            }

            #endregion

            #region Reestablecer de conjuntos

            for (int i = listaConjunto.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < listaNombres.Count; j++)
                    listaNombres[j] = listaNombres[j].Replace(string.Format("#{0}", i), listaConjunto[i]).Trim();
                for (int j = 0; j < listaApellidos.Count; j++)
                    listaApellidos[j] = listaApellidos[j].Replace(string.Format("#{0}", i), listaConjunto[i]).Trim();
            }

            #endregion

            return new NombreApellidoSeparado()
            {
                Apellidos = listaApellidos,
                Nombres = listaNombres,
                Apellido = string.Join(" ", listaApellidos.ToArray()),
                Nombre = string.Join(" ", listaNombres.ToArray()),
            };
        }

        public class NombreApellidoSeparado
        {
            public List<string> Nombres { get; set; } = new();
            public List<string> Apellidos { get; set; } = new();
            public string Nombre { get; set; } = string.Empty;
            public string Apellido { get; set; } = string.Empty;

        }

        public static string Indice(this IList<string> lista, int indice)
        {
            if (lista == null || lista.Count() <= indice) return string.Empty;
            return lista[indice];
        }

        public static string LimpiarCaracteresParaXml(this string? value, bool enMayusculas = false)
        {
            if (enMayusculas)
                value = value.ReplaceIfNullOrEmpty().ToUpper();

            Dictionary<string, string> caracteresReemplazo = new()
            {
                { "Á", "A"},
                { "É", "E"},
                { "Í", "I"},
                { "Ó", "O"},
                { "Ú", "U"},
                { "Ñ", "N"},
                { "-", " "},
                { "&", "Y"},
                { "/", " "},
                { ":", " "},
            };

            foreach (var item in caracteresReemplazo)
            {
                value = value.ReplaceIfNullOrEmpty().Replace(item.Key, item.Value, StringComparison.InvariantCultureIgnoreCase);
            }


            return value.ReplaceIfNullOrEmpty();
        }

        public static string Format(this string formato, params string[] argumentos)
        {
            return string.Format(formato, argumentos);
        }

        public static DateTime? FromUnixTime(this long? milisegundosUnixTimestamp)
        {
            return milisegundosUnixTimestamp.HasValue ? milisegundosUnixTimestamp.GetValueOrDefault().FromUnixTime() : null;
        }

        public static DateTime FromUnixTime(this long milisegundosUnixTimestamp)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(milisegundosUnixTimestamp);
            return dateTimeOffset.LocalDateTime;
        }

        public static bool ConvertObjectToBoolean(this object? objeto)
        {
            string datoObjeto = (objeto == null || objeto.ToString() == "" ? "0" : objeto.ToString()!).ReplaceIfNullOrEmpty("0").ToLower().Trim();
            bool resultado = (new string[] { "1", "true", "yes", "y", "ok", "verdadero", "si", "s", "acepto" }).Contains(datoObjeto);
            return resultado;
        }

        public static List<T> DeserializarContenidoArchivo<T>(this string contenido, params char[] separadores) where T : new()
        {
            var lineas = contenido.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var headers = lineas[0].Split(separadores, StringSplitOptions.RemoveEmptyEntries);
            var registros = new List<T>();

            foreach (var line in lineas.Skip(1))
            {
                var values = line.Split(separadores, StringSplitOptions.TrimEntries);
                var registro = new T();

                for (int i = 0; i < headers.Length; i++)
                {
                    var property = typeof(T).GetProperty(headers[i]);
                    if (property != null)
                    {
                        object convertedValue = Convert.ChangeType(values[i], property.PropertyType);
                        property.SetValue(registro, convertedValue);
                    }
                }

                registros.Add(registro);
            }

            return registros;
        }

        public static DateTime DefaultIfEmpty(this DateTime? fecha) => fecha.DefaultIfEmpty(DateTime.MinValue);

        public static DateTime DefaultIfEmpty(this DateTime? fecha, DateTime valorDefault)
        {
            if (fecha == null)
                return valorDefault;
            else
                return fecha.Value;
        }


        public static int Length(this int @this) => @this.ToString().Length;

        public static int Length(this double @this) => @this.ToString().Length;


        public static string RecortarTexto(this string @this, int longitudMaxima, bool puntosSuspensivos = false)
            => !@this.IsNullOrEmpty() && @this.Length > longitudMaxima
            ? $"{@this[..(longitudMaxima - (puntosSuspensivos ? 3 : 0))]}{(puntosSuspensivos ? "..." : "")}"
            : @this.ToString();


        public static string PresentacionValores(this decimal @this,
                                                 bool conDecimales = true,
                                                 int? numeroDecimales = null,
                                                 bool esMoneda = true)
        {
            var formato = $"{(esMoneda ? "C" : "N")}{(conDecimales ? numeroDecimales.GetValueOrDefault(DecimalesAmbysoft) : 0)}";
            return @this.ToString(formato, new CultureInfo(CulturaAmbysoft));
        }

        public static string ConvierteBooleanSiNo(this bool @this)
        {
            return @this ? "SI" : "NO";
        }

        public static string NormalizeAccentInsensitive(this string @this)
            => !@this.IsNullOrEmpty()
            ? new string(@this.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .ToLowerInvariant()
            : string.Empty;

        public static bool EqualsIgnoreAccentsAndCase(this string input, string comparacion)
        {
            var valor = input.IsNullOrEmpty() ? string.Empty : input;
            var valorComparar = comparacion.IsNullOrEmpty() ? string.Empty : comparacion;
            return valor.NormalizeAccentInsensitive() == valorComparar.NormalizeAccentInsensitive();
        }

        public static string LimpiarNombreArchivo(this string nombreArchivo)
        {
            if (string.IsNullOrEmpty(nombreArchivo))
                return string.Empty;

            // Permitir: letras, números, espacio, punto, guion y guion_bajo
            return Regex.Replace(nombreArchivo, @"[^a-zA-Z0-9_ \.\-]", "")
                .Replace("  ", " ")
                .Replace(" .", ".")
                .Trim();
        }

        public static TimeSpan ConvertirDesdeAmbysoftTimeSpan(this string objeto)
        {
            string[] componentes = objeto.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            int d = (componentes.FirstOrDefault(x => x.Contains("d", StringComparison.CurrentCultureIgnoreCase))?.Replace("d", "", StringComparison.CurrentCultureIgnoreCase).ReplaceIfNullOrEmpty("0")).ConvertObjectToInt();

            int h = (componentes.FirstOrDefault(x => x.Contains("h", StringComparison.CurrentCultureIgnoreCase))?.Replace("h", "", StringComparison.CurrentCultureIgnoreCase).ReplaceIfNullOrEmpty("0")).ConvertObjectToInt();
            int m = (componentes.FirstOrDefault(x => x.Contains("m", StringComparison.CurrentCultureIgnoreCase))?.Replace("m", "", StringComparison.CurrentCultureIgnoreCase).ReplaceIfNullOrEmpty("0")).ConvertObjectToInt();

            return new TimeSpan(d, h, m, 0);
        }

        public static DateTime ExtraerDesdeAmbysoftDateTime(this DateTime fechaOrigen, string parametroTimeSpan)
        {
            return fechaOrigen.Subtract(parametroTimeSpan.ConvertirDesdeAmbysoftTimeSpan());
        }

        public static Decimal ConvertirDesdeAmbysoftDecimal(this string objeto, out bool esPorcentaje)
        {
            esPorcentaje = false;
            decimal valor = objeto.ReplaceIfNullOrEmpty("0").ConvertObjectToDecimal();
            if (objeto.ReplaceIfNullOrEmpty("0").EndsWith("%"))
            {
                esPorcentaje = true;
                return valor / 100M;
            }
            return valor;
        }

        public static decimal ExtraerValorDesdeAmbysoftDecimal(this decimal? valorOrigen, string parametroValorPorcentaje)
        {
            var valorPorcentaje = parametroValorPorcentaje.ConvertirDesdeAmbysoftDecimal(out bool esPorcentaje);
            if (esPorcentaje)
                return valorOrigen.GetValueOrDefault() * valorPorcentaje;

            return valorPorcentaje;
        }
    }

}