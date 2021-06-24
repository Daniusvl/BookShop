using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Books.Tests
{
    public class BooksDbTests
    {
        public async Task Create_Author_Test()
        {
            DbContextOptionsBuilder<BooksDb> options = new();
            options.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            BooksDb context = new(options.Options, null, null);
        }
    }
}
