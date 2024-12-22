using AspNetMvcCacheRedis.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace AspNetMvcCacheRedis.Controllers;

public class HomeController : Controller
{
    private TimeSpan expDate => TimeSpan.FromSeconds(120);
    private readonly IRedisCacheService _redisService;
    public HomeController(IRedisCacheService redisService)
    {
        _redisService = redisService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetVal(string key)
    { 
        string val = await _redisService.GetCacheValueAsync(key);
        return Json(val);
    }

    public async Task<IActionResult> SetVal(string key, string val)
    {
        await _redisService.SetCacheValueAsync(key, val, expDate);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DelVal(string key)
    {
        await _redisService.DeleteCacheValueAsync(key);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> AppendVal(string key, string val)
    {
        await _redisService.AppendCacheValueAsync(key, val);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> SetHashVal(string hashName, string fieldName, string value)
    {
        await _redisService.SetHashValueAsync(hashName, fieldName, value);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> GetHashVal(string hashName, string field)
    {
        int data = await _redisService.GetHashEntryAsync(hashName, field);
        return Json(data + 1);
    }
}
