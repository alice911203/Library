using Microsoft.EntityFrameworkCore;
using booksystem.api.Models;

namespace booksystem.api.Data
{
    public class ApplicationDbContext: DbContext //設定一個類別，並讓她繼承DbContext類別(處理底層連線用的)
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options //設定options接收資料庫連線字串，並將其泛型設定為ApplicationDbContext
            ) : base(options) //將接收到的options資訊傳回DbContext類別作處理
        {

        }
        public DbSet<Book> Books { get; set; }//設定一張Books資料表，並設定其欄位規範跟Book類別一致
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  Category 被刪除時，把 Book 裡面的 CategoryId 設為 Null (SetNull)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
