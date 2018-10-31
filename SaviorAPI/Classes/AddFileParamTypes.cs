﻿using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace SaviorAPI
{
    public class AddFileParamTypes : IOperationFilter
    {
        Parameter fileParameter = new Parameter
        {
            name = "file",
            required = true,
            description = "An Image file",
            type = "file",
            @in = "formData",
            vendorExtensions = new Dictionary<string, object> { { "x-ms-media-kind", "image" } }
        };
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.operationId == "Upload" 
                || operation.operationId == "Describe" 
                || operation.operationId == "Detect"
                || operation.operationId == "RecognizeText"
                || operation.operationId == "RecognizeHandWrittenText"
                || operation.operationId == "GetTags"
                || operation.operationId == "Analyze"
                || operation.operationId == "GetThumbnail"
                || operation.operationId == "Predict"
                )  // controller and action name
            {
                operation.consumes.Add("multipart/form-data");
                if(operation.parameters == null)
                {
                    operation.parameters = new List<Parameter> { fileParameter };
                }
                else
                {
                    operation.parameters.Add(fileParameter);
                }                
            }
        }
    }
}