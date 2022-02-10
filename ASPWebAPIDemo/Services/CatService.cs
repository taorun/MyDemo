using ASPWebAPIDemo.Models;

namespace ASPWebAPIDemo.Services
{
    public static class CatService
    {
        static List<Cat> Cats { get; }
        static CatService()
        {
            Cats = new List<Cat> { };
        }

        public static List<Cat> GetAll() => Cats;
        public static void Add(Cat cat)
        {
            Cats.Add(cat);
        }
    }
}
