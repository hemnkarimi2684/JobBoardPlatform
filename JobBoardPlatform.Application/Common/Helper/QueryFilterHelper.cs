using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.Linq.Expressions;

namespace JobBoardPlatform.Application.Common.Helper;

public static class QueryFilterHelper
{
    public static Expression<Func<Advertisement, bool>> BuildSearchFilterPredicate(
        AdvertisementSearchRequestDto searchDto,
        AdvertisementFilterRequestDto filterDto)
    {
        // اینجا داره میگه پارمتر رو از نوع a  بخون 
        var parameter = Expression.Parameter(typeof(Advertisement), "a");

        Expression? combined = null;

        // اینجا میگه اگه مقدار وروردی برای سرچ خالی نبود وارد شو
        if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
        {
            //اینجا میاد هرچی فضای خالی اطرافش رو پاک میکنه
            var term = searchDto.SearchTerm.Trim();

            // اینجا میاد با استافده از رفلکشن متد کانتینز رو دریافت میکنه به این صورت که میگه 
            // برو توی متد های کلاس استرینگ متد به اسم کانتینز رو با اور لود پارامتر ورودی پایین بیار چون 
            // خود متد ممکنه چندین اور لود داشته باشه 
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });

            // حالا اینجا میاد اون رشته ورودی رو به یک ثابت توی اکسپرژن تبدیل میکنه
            // الان این مقدار بعدا اینجوری توی اکسپرژن استفاده میشه 
            // term = "asp.net" => Exprssion.Constant("asp.net") => a => a.Title.Contains("asp.net")
            var termConstant = Expression.Constant(term);

            // اینجا کد یک شرط داینامیک برای بررسی کانتینز روی نام شغل می سازد
            // فرض کن توی انتیتی اگهی چیزی به اسم job داری و میخوای رو ی job Name  سرچ بزنی 
            // این کد در نهایت همچین چیزی میسازه a => a.Job.Name.Contains(term)
            // حالا چجوری میسازه ؟ 
            //اولین ورودی میره پارمتر ورودی میده که میگه با چی باید مقایسه شه 
            // الان میره توی انتیتی اگهی میره شغل رو میاره از طریق اون به اسمش میرسه حالا این میگه روی اسم شغل سرچ میخوره
            // حالا ورودی های دوم و سوم میگه که روی مقدار اسم شغل متد کانتینز را با مقدار ثابت سرچ اجرا کن
            var jobLike = Expression.Call(
                Expression.Property(Expression.Property(parameter, nameof(Advertisement.Job)), nameof(Advertisement.Job.Name)),
                containsMethod,
                termConstant);

            var cityLike = Expression.Call(
                Expression.Property(Expression.Property(parameter, nameof(Advertisement.City)), nameof(Advertisement.City.Name)),
                containsMethod,
                termConstant);

            var companyLike = Expression.Call(
                Expression.Property(Expression.Property(parameter, nameof(Advertisement.Company)), nameof(Advertisement.Company.Name)),
                containsMethod,
                termConstant);

            // این میاد سه تا شرط رو باهم به صورت یا ترکیب میکنه 
            // jobLike || cityLike || companyLike
            combined = Expression.OrElse(jobLike, Expression.OrElse(cityLike, companyLike));
        }

        if (filterDto.JobCategoryId.HasValue)
        {

            // خب اینجا میره ویژگی ایدی دسته بندی شغلی رو میاره از طریق رفلکشن 
            var jobCategoryId = Expression.Property(
                Expression.Property(parameter, nameof(Advertisement.Job)),
                nameof(Advertisement.Job.JobCategoryId));

            // حالا اینجا میگه که از دسته بندی شغلی گرفته با مقدار ورودی فیلتر دسته بندی شغلی برابره یا نه 
            var equal = Expression.Equal(jobCategoryId, Expression.Constant(filterDto.JobCategoryId.Value));

            // اینجا هم میگه اگه نال بود اون شرط ها و تا الان شرطی نساخته شده بود همین فیلتر رو شرط اصلی کن 
            // combind  رو با همین شرط توی ایف فیلتر پر کن در غیر این صورت اینم به صورت and اضافه کن
            combined = combined is null ? equal : Expression.AndAlso(combined, equal);
        }

        if (filterDto.CollaborationType.HasValue)
        {
            var collaborationType = Expression.Property(parameter, nameof(Advertisement.CollaborationType));

            var equal = Expression.Equal(collaborationType, Expression.Constant(filterDto.CollaborationType.Value));

            combined = combined is null ? equal : Expression.AndAlso(combined, equal);
        }

        if (filterDto.MinimumSalary.HasValue)
        {
            var minimumSalary = Expression.Property(parameter, nameof(Advertisement.MinimumSalary));

            // اینجاهم چیزی که گرفته رو مقایسه میکنه بزرگتر مساویه یا نه 
            var greaterOrEqual = Expression.GreaterThanOrEqual(minimumSalary, Expression.Constant(filterDto.MinimumSalary.Value));

            combined = combined is null ? greaterOrEqual : Expression.AndAlso(combined, greaterOrEqual);
        }

        if (filterDto.MaximumSalary.HasValue)
        {
            var maximumSalary = Expression.Property(parameter, nameof(Advertisement.MaximumSalary));

            // اینجاهم چیزی که گریفته رو مقایسه میکنه کوچک تر مساویه یا نه 
            var lessOrEqual = Expression.LessThanOrEqual(maximumSalary, Expression.Constant(filterDto.MaximumSalary.Value));

            combined = combined is null ? lessOrEqual : Expression.AndAlso(combined, lessOrEqual);
        }

        var isActiveProperty = Expression.Property(parameter, nameof(Advertisement.IsActive));
        var isActiveEqual = Expression.Equal(isActiveProperty, Expression.Constant(true));

        combined = combined is null ? isActiveEqual : Expression.AndAlso(combined, isActiveEqual);

        var statusProperty = Expression.Property(parameter, nameof(Advertisement.Status));
        var statusOpen = Expression.Equal(statusProperty, Expression.Constant(AdvertisementStatus.Open));

        combined = combined is null ? statusOpen : Expression.AndAlso(combined, statusOpen);

        //آخرین قطعه از پازل ساختن Expression Tree داینامیک
        // اینجا lamba کارش اینه که 
        //تمام شرط هایی که ساخته شده رو اون رو به یک تابع شرطی یا لامبادا اکسپرژن تبدیل کنه که قابل فهم برای ای اف و تبدیل لینک به اس کیو ال باشه 
        return Expression.Lambda<Func<Advertisement, bool>>(combined, parameter);
    }
}
