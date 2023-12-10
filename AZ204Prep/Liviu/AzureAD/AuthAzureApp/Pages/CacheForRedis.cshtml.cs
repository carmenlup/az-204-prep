using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using StackExchange.Redis;
using System.Data.Common;

namespace AuthAzureApp.Pages
{
    public class CacheForRedis : PageModel
    {
        public List< string>? CachedValues;
        public const string c_cacheKey = "top3Courses";

        [BindProperty]
        public string CacheKey { get; set; }

        [BindProperty]
        public string CacheValue { get; set; }

        [BindProperty]
        public string CacheKeyDisplay { get; set; }
        public IConnectionMultiplexer Redis { get; }

        public CacheForRedis(IConnectionMultiplexer redis)
        {

            Redis = redis;
            CachedValues = new List< string>();
            var db = redis.GetDatabase();
            if (!db.KeyExists(c_cacheKey))
            {
                db.ListRightPush(c_cacheKey, new[] {new RedisValue("Course 11"), new RedisValue("Course 22") , new RedisValue("Course 33") });
            }
        }


        public async Task OnGet()
        {

            //CachedValues = await GetValuesFromCache(c_cacheKey);

        }

        public async Task<List<string>> GetValuesFromCache(string key)
        {
            var db = Redis.GetDatabase();
            return (await db.ListRangeAsync(key)).Select(x => x.ToString()).ToList();
        }

        public async Task<bool> Exists(string key)
        {
            var db = Redis.GetDatabase();
            return await db.KeyExistsAsync(key);
        }

        public void SetKeyExpiry(string key, TimeSpan time)
        {
            var db = Redis.GetDatabase();
            db.KeyExpire(key, time);
        }

        public async Task OnPostSetCacheAsync()
        {
            var db = Redis.GetDatabase();
            //await db.StringSetAsync(CacheKey, CacheValue);
            await db.ListRightPushAsync(CacheKey, new[] { new RedisValue(CacheValue) });
            SetKeyExpiry(CacheKey, new TimeSpan(0, 0, 30));
        }

        public async Task<IActionResult> OnPostDisplayAsync()
        {
            bool keyExists = await Exists(CacheKeyDisplay);
            CachedValues = keyExists ? await GetValuesFromCache(CacheKeyDisplay) : null;

            return Page();
        }
    }
}
