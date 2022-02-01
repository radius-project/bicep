// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure;
using Azure.Containers.ContainerRegistry;
using Azure.Containers.ContainerRegistry.Specialized;
using Azure.Core;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bicep.Core.Registry
{
    public class DynamicCredentialRegistryClient : ContainerRegistryBlobClient
    {
        private ContainerRegistryBlobClient current;
        private bool credentialsInitialized;
        private object? @lock;

        public DynamicCredentialRegistryClient(Uri endpoint, string repository, ContainerRegistryClientOptions options, Func<TokenCredential> createCredential)
        {
            // Try anonymous access first
            this.current = new ContainerRegistryBlobClient(endpoint, repository, options);
        }

        public override Uri Endpoint => current.Endpoint;

        public override string RepositoryName => current.RepositoryName;

        private T RetryWithCredentialsSync<T>(Func<ContainerRegistryBlobClient, T> func)
        {
            var client = this.current;
            if (!credentialsInitialized)
            {
                try
                {
                    return func(client);
                }
                catch (RequestFailedException ex) when (ex.Status == 401)
                {
                    // This will happen when the registry does not support anonymous access.
                    // Get credentials and try again.
                }
            }

            client = LazyInitializer.EnsureInitialized<ContainerRegistryBlobClient>(ref current, ref credentialsInitialized, ref @lock);
            return func(client);
        }

        private async Task<T> RetryWithCredentialsAsync<T>(Func<ContainerRegistryBlobClient, Task<T>> func)
        {
            var client = this.current;
            if (!credentialsInitialized)
            {
                try
                {
                    return await func(client);
                }
                catch (RequestFailedException ex) when (ex.Status == 401)
                {
                    // This will happen when the registry does not support anonymous access.
                    // Get credentials and try again.
                }
            }

            client = LazyInitializer.EnsureInitialized<ContainerRegistryBlobClient>(ref current, ref credentialsInitialized, ref @lock);
            return await func(client);
        }

        public override Response DeleteBlob(string digest, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsSync<Response>(client => client.DeleteBlob(digest, cancellationToken));
        }

        public override Task<Response> DeleteBlobAsync(string digest, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsAsync<Response>(client => client.DeleteBlobAsync(digest, cancellationToken));
        }

        public override Response DeleteManifest(string digest, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsSync<Response>(client => client.DeleteManifest(digest, cancellationToken));
        }

        public override Task<Response> DeleteManifestAsync(string digest, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsAsync<Response>(client => client.DeleteManifestAsync(digest, cancellationToken));
        }

        public override Response<DownloadBlobResult> DownloadBlob(string digest, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsSync<Response<DownloadBlobResult>>(client => client.DownloadBlob(digest, cancellationToken));
        }

        public override Task<Response<DownloadBlobResult>> DownloadBlobAsync(string digest, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsAsync<Response<DownloadBlobResult>>(client => client.DownloadBlobAsync(digest, cancellationToken));
        }

        public override Response<DownloadManifestResult> DownloadManifest(DownloadManifestOptions options, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsSync<Response<DownloadManifestResult>>(client => client.DownloadManifest(options, cancellationToken));
        }

        public override Task<Response<DownloadManifestResult>> DownloadManifestAsync(DownloadManifestOptions options, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsAsync<Response<DownloadManifestResult>>(client => client.DownloadManifestAsync(options, cancellationToken));
        }

        public override Response<UploadBlobResult> UploadBlob(Stream stream, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsSync<Response<UploadBlobResult>>(client => client.UploadBlob(stream, cancellationToken));
        }

        public override Task<Response<UploadBlobResult>> UploadBlobAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsAsync<Response<UploadBlobResult>>(client => client.UploadBlobAsync(stream, cancellationToken));
        }

        public override Response<UploadManifestResult> UploadManifest(OciManifest manifest, UploadManifestOptions? options = null, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsSync<Response<UploadManifestResult>>(client => client.UploadManifest(manifest, options, cancellationToken));
        }

        public override Response<UploadManifestResult> UploadManifest(Stream manifestStream, UploadManifestOptions? options = null, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsSync<Response<UploadManifestResult>>(client => client.UploadManifest(manifestStream, options, cancellationToken));
        }

        public override Task<Response<UploadManifestResult>> UploadManifestAsync(OciManifest manifest, UploadManifestOptions? options = null, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsAsync<Response<UploadManifestResult>>(client => client.UploadManifestAsync(manifest, options, cancellationToken));
        }

        public override Task<Response<UploadManifestResult>> UploadManifestAsync(Stream manifestStream, UploadManifestOptions? options = null, CancellationToken cancellationToken = default)
        {
            return RetryWithCredentialsAsync<Response<UploadManifestResult>>(client => client.UploadManifestAsync(manifestStream, options, cancellationToken));
        }
    }
}
