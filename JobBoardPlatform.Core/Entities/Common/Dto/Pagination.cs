namespace JobBoardPlatform.Core.Entities.Common.Dto;

public class Pagination<T>
{
    /// <summary>
    /// لیست دیتا خواسته شده 
    /// </summary>
    public List<T> Data { get; set; } = new();

    /// <summary>
    /// شماره صفحه
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// تعداد دیتا توی هر صفحه 
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// نعداد کل صفحه ها
    /// </summary>
    public int TotalPageCount { get; set; }

    public static Pagination<T> GetPagination(List<T> data, int pageNumber, int pageSize, int totalcountData)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        // این فرمولش برای گردن کردن رو بالا
        //فرض کن تعداد دیتا ها 100 تعداد سایز هر صفحه 10 
        //حالا این اگه باهم دیگه جمع بشه میشه 110 
        //اگه این رو مستقیم تقسیم بر تعداد سایز کنیم یعنی 10 میشه 11 صفحه کلا که این اشتباهه
        //حالا فرض کن که قبل تقسیم منهای 1 انجام بشه این 109 حالا اگر 109 رو تقسیم بر تعداد سایز کنیم میشه 10 
        //پس این یک عدد کم کردن برای اینه که گردن کردن رو به بالا درست انجام بشه 
        var totalPageCount = totalcountData == 0 ? 0 : (totalcountData + pageSize - 1) / pageSize;

        return new Pagination<T>()
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPageCount = totalPageCount
        };
    }
}

