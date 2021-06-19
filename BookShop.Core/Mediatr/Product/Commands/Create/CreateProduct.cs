using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Product.Commands.Create
{
    public static class CreateProduct
    {
        public record Command(Domain.Entities.Product Product, IList<byte> Bytes) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository repository;
            private readonly IConfiguration configuration;

            public Handler(IProductRepository repository, IConfiguration configuration)
            {
                this.repository = repository;
                this.configuration = configuration;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if(repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                if(configuration == null)
                {
                    throw new ServiceNullException(nameof(IConfiguration), nameof(Handler));
                }

                if (configuration.IsDevelopment())
                {
                    if(request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }

                    if(request.Product == null)
                    {
                        throw new ArgumentNullException(nameof(request.Product));
                    }
                }

                // TODO: Add validation and logging.

                string path = Directory.GetCurrentDirectory() + $@"\Books\{Guid.NewGuid()}.pdf";

                while (File.Exists(path))
                {
                    path = Directory.GetCurrentDirectory() + $@"\Books\{Guid.NewGuid()}.pdf";
                }

                await File.WriteAllBytesAsync(path, request.Bytes.ToArray());

                request.Product.FilePath = path;

                await repository.Create(request.Product);

                return default;
            }
        }
    }
}
