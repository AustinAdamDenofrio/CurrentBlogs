using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Services.Interfaces;
using CurrentBlogs.Components;
using CurrentBlogs.Components.Account;
using CurrentBlogs.Data;
using CurrentBlogs.Services;
using CurrentBlogs.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = DataUtility.GetConnectionString(builder.Configuration) 
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    ));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

//opt out of email confirmation
//builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, SendGridService>();
builder.Services.AddSingleton<IEmailSender, SendGridService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryDTOService, CategoryDTOService>();

builder.Services.AddScoped<IBlogPostsRepository, BlogPostsRepository>();
builder.Services.AddScoped<IBlogPostDTOService, BlogPostDTOService>();

builder.Services.AddScoped<ICommentsRepository, CommentRepository>();
builder.Services.AddScoped<ICommentsDTOService, CommentDTOService>();


builder.Services.AddCors(builder =>
{
    builder.AddPolicy("DefaultPolicy", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("DefaultPolicy");


using (var scope = app.Services.CreateScope())
{
    await DataUtility.ManageDataAsync(scope.ServiceProvider);
}

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CurrentBlogs.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.


app.MapAdditionalIdentityEndpoints();

app.MapControllers();

// GET: api/blogposts
app.MapGet("api/blogposts", async ([FromServices] IBlogPostDTOService blogService,
                                   [FromQuery] int page = 1,
                                   [FromQuery] int pageSize = 4) =>
{
    try
    {
        PagedList<BlogPostDTO> blogPosts = await blogService.GetPublishedBlogPostsAsync(page, pageSize);
        return Results.Ok(blogPosts);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        return Results.Problem();
    }
});



app.Run();
