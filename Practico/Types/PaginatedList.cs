using Microsoft.EntityFrameworkCore;

namespace Practico.Types
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public PaginatedInfo Info { get; set; }
        public int Pages { get; private set; }



        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            Pages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < Pages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }


        public void SetInfo(string baseUrl, int totalP, int cantidadP, int paginaActual, int pageSize, int? instanciaId, string searchText = "")
        {

            string anterior = null;
            string siguiente = null;

            string instanciaParameter = instanciaId.HasValue ? $"instanciaid={instanciaId}&" : "";
            string searchTextParameter = string.IsNullOrWhiteSpace(searchText) ? "" : $"&searchText={Uri.EscapeDataString(searchText)}";

            if (totalP == paginaActual)
            {
                if(totalP != 1)
                    anterior = $"{baseUrl}?{instanciaParameter}page={paginaActual - 1}&pageSize={pageSize}{searchTextParameter}";
            }
            else if (paginaActual == 1)
            {
                siguiente = $"{baseUrl}?{instanciaParameter}page={paginaActual + 1}&pageSize={pageSize}{searchTextParameter}";
            }
            else
            {
                siguiente = $"{baseUrl}?{instanciaParameter}page={paginaActual + 1}&pageSize={pageSize}{searchTextParameter}";
                anterior = $"{baseUrl}?{instanciaParameter}page={paginaActual - 1}&pageSize={pageSize}{searchTextParameter}";
            }

            Info = new PaginatedInfo
            {
                Count = cantidadP,
                Pages = totalP,
                Next = siguiente,
                Prev = anterior
            };
        }


    }


}
