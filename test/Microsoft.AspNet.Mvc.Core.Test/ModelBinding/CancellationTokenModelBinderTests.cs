// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Xunit;

namespace Microsoft.AspNet.Mvc.ModelBinding.Test
{
    public class CancellationTokenModelBinderTests
    {
        [Fact]
        public async Task CancellationTokenModelBinder_ReturnsTrue_ForCancellationTokenType()
        {
            // Arrange
            var bindingContext = GetBindingContext(typeof(CancellationToken));
            var binder = new CancellationTokenModelBinder();

            // Act
            var bound = await binder.BindModelAsync(bindingContext);

            // Assert
            Assert.NotNull(bound);
            Assert.Equal(bindingContext.OperationBindingContext.HttpContext.RequestAborted, bound.Model);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(object))]
        [InlineData(typeof(CancellationTokenModelBinderTests))]
        public async Task CancellationTokenModelBinder_ReturnsFalse_ForNonCancellationTokenType(Type t)
        {
            // Arrange
            var bindingContext = GetBindingContext(t);
            var binder = new CancellationTokenModelBinder();

            // Act
            var bound = await binder.BindModelAsync(bindingContext);

            // Assert
            Assert.Null(bound);
        }

        private static ModelBindingContext GetBindingContext(Type modelType)
        {
            var metadataProvider = new EmptyModelMetadataProvider();
            ModelBindingContext bindingContext = new ModelBindingContext
            {
                ModelMetadata = metadataProvider.GetMetadataForType(modelType),
                ModelName = "someName",
                ValueProvider = new SimpleHttpValueProvider(),
                OperationBindingContext = new OperationBindingContext
                {
                    ModelBinder = new CancellationTokenModelBinder(),
                    MetadataProvider = metadataProvider,
                    HttpContext = new DefaultHttpContext(),
                }
            };

            return bindingContext;
        }
    }
}
