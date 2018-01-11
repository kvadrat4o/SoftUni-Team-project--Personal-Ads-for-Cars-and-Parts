namespace Store.Services
{
    using Store.Data.Models.Enums;

    public class Paginator<TModel>
    {
        public string PageTitle { get; set; }

        public TModel Model { get; set; }

        public int CurrentPage { get; set; }

        public int AllPages { get; set; }

        public string ActionName { get; set; } = ServiceConstants.PaginationDefaultActionName;

        public Category? Category { get; set; }
    }
}
