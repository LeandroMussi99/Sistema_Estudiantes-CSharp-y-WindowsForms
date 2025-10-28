using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Library
{
    public class Paginador<T>
    {
        private List<T> dataList;
        private Label label;
        private static int maxReg, regPorPag, pageCount, numPag=1;

        public Paginador(List<T> dataList, Label label, int regPorPag)
        {
            this.dataList = dataList;
            this.label = label;
            Paginador<T>.regPorPag = regPorPag;
            cargarDatos();
        }

        private void cargarDatos()
        {
            numPag = 1;
            maxReg = dataList.Count;
            pageCount = Math.Max(1, (int)Math.Ceiling(maxReg / (double)regPorPag));
            label.Text = $"Pagina {numPag} de {pageCount}";
        }

        public int primero() { numPag = 1; label.Text = $"Pagina {numPag} de {pageCount}"; return 0; }
        public int anterior() { if (numPag > 1) numPag--; label.Text = $"Pagina {numPag} de {pageCount}"; return (numPag - 1) * regPorPag; }
        public int siguiente() { if (numPag < pageCount) numPag++; label.Text = $"Pagina {numPag} de {pageCount}"; return (numPag - 1) * regPorPag; }
        public int ultimo() { numPag = pageCount; label.Text = $"Pagina {numPag} de {pageCount}"; return (numPag - 1) * regPorPag; }

    }
}
