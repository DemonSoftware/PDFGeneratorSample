using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using System.IO;
using System;
using Microsoft.OpenApi.Models;
using PDFGeneratorSample.Models;
using PDFGeneratorSample.Samples;

// Register QuestPDF license (Community Edition)
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PDF Generator API",
        Description = "API for generating PDF documents using QuestPDF",
        Version = "v1"
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDirectoryBrowser();

var app = builder.Build();
app.UseStaticFiles(); 
app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PDF Generator API V1");
    c.RoutePrefix = "swagger";
    
    // Add a link to the PDF endpoints
    c.HeadContent = @"
        <div style='padding: 15px; background-color: #f8f9fa; margin-bottom: 10px; border-radius: 5px;'>
            <h3>Quick PDF Links:</h3>
            <ul>
                <li><a href='/generate-pdf' target='_blank'>Download PDF</a></li>
                <li><a href='/preview-pdf' target='_blank'>Preview PDF</a></li>
            </ul>
        </div>";
});

// Enable static files if wwwroot exists
if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")))
{
    app.UseStaticFiles();
    // Add a route to HTML page if it exists
    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html")))
    {
        app.MapGet("/pdf-ui", () => Results.Redirect("/index.html"));
    }
}

app.MapGet("/", () => Results.Redirect("/index.html"));

// PDF Generator endpoint with Swagger documentation
app.MapGet("/generate-pdf", async (HttpContext context) =>
    {
        // Create sample data for the tables
        var firstTableData = SampleData.GetFirstTableData();
        var secondTableData = SampleData.GetSecondTableData();

        // Create the document
        var document = new SampleDocument.MyDocument(firstTableData, secondTableData);
    
        // Generate PDF to memory stream
        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;
    
        // Return PDF as downloadable file
        context.Response.ContentType = "application/pdf";
        context.Response.Headers.Add("Content-Disposition", "attachment; filename=report.pdf");
        await stream.CopyToAsync(context.Response.Body);
    
        return Results.Empty;
    })
    .WithName("GeneratePDF")
    .WithOpenApi(operation => 
    {
        operation.Summary = "Generate a PDF report";
        operation.Description = "Creates a PDF with a title page and two tables, and returns it as a downloadable file";
        return operation;
    });

// PDF Preview endpoint - displays PDF in browser instead of downloading
app.MapGet("/preview-pdf", async (HttpContext context) =>
    {
        // Create sample data for the tables
        var firstTableData = SampleData.GetFirstTableData();
        var secondTableData = SampleData.GetSecondTableData();

        // Create the document
        var document = new SampleDocument.MyDocument(firstTableData, secondTableData);
    
        // Generate PDF to memory stream
        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;
    
        // Return PDF to be displayed in browser
        context.Response.ContentType = "application/pdf";
        context.Response.Headers.Add("Content-Disposition", "inline");
        await stream.CopyToAsync(context.Response.Body);
    
        return Results.Empty;
    })
    .WithName("PreviewPDF")
    .WithOpenApi(operation => 
    {
        operation.Summary = "Preview a PDF report";
        operation.Description = "Creates a PDF with a title page and two tables, and displays it in the browser";
        return operation;
    });

app.Run();