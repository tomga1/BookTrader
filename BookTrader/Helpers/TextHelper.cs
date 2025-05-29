using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace BookTrader.Helpers
{
    public class TextHelper
    {
        public static string NormalizarNombreLibro(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return texto;
            }

            texto = texto.ToUpperInvariant();

            var textoNormalizado = texto.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();

            foreach (var c in textoNormalizado)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            texto = sb.ToString().Normalize(NormalizationForm.FormC);

            texto = Regex.Replace(texto, @"[^A-Z0-9 ]", "");

            return texto;

        }   
    }
}
