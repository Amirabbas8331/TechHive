using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) ||
                        p.ParameterType == typeof(IFormFileCollection))
            .ToList();

        if (!fileParameters.Any())
            return;

        // ساخت schema برای object
        var properties = new Dictionary<string, IOpenApiSchema>();
        var required = new HashSet<string>();

        foreach (var param in fileParameters)
        {
            string paramName = param.Name ?? "file";

            // اگر IFormFileCollection باشد، معمولاً به صورت آرایه آپلود می‌شود
            if (param.ParameterType == typeof(IFormFileCollection))
            {
                properties["files"] = new OpenApiSchema
                {
                    Type = JsonSchemaType.Array,
                    Items = new OpenApiSchema
                    {
                        Type = JsonSchemaType.String,
                        Format = "binary"
                    },
                    Description = "لیست فایل‌ها برای آپلود"
                };
                required.Add("files");
            }
            else // IFormFile تک
            {
                properties[paramName] = new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Format = "binary",
                    Description = $"فایل {paramName} برای آپلود"
                };
                required.Add(paramName);
            }
        }

        var schema = new OpenApiSchema
        {
            Type = JsonSchemaType.Object,
            Properties = properties,
            Required = required
        };

        operation.RequestBody = new OpenApiRequestBody
        {
            Description = "آپلود فایل با فرمت multipart/form-data",
            Required = true,
            Content =
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = schema
                }
            }
        };
    }
}