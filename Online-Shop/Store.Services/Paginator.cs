namespace Store.Services
{
    public class Paginator<TModel>
    {
        public string PageTitle { get; set; }

        public TModel Model { get; set; }

        public int CurrentPage { get; set; }

        public int AllPages { get; set; }
    }
}
