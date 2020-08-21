// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Linq;
using Azure.Deployments.Expression.Expressions;
using Bicep.Core.Extensions;
using Bicep.Core.Parsing;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;

namespace Bicep.Core.Emit
{
    public partial class ExpressionConverter
    {
        public LanguageExpression GetResourceNameExpression(CompoundName name)
        {
            return ConvertString(name);
        }

        public LanguageExpression GetResourceNameExpression(DeclaredSymbol symbol)
        {
            return symbol switch
            {
                ResourceSymbol resourceSymbol => GetResourceNameExpression(resourceSymbol),
                ApplicationSymbol applicationSymbol => GetResourceNameExpression(applicationSymbol),
                ComponentSymbol componentSymbol => GetResourceNameExpression(componentSymbol),
                DeploymentSymbol deploymentSymbol => GetResourceNameExpression(deploymentSymbol),

                _ => throw new InvalidOperationException($"Unexpected symbol type {symbol.GetType()}"),
            };
        }

        public LanguageExpression GetResourceNameExpression(ApplicationSymbol applicationSymbol)
        {
            // this condition should have already been validated by the type checker
            var nameValueSyntax = applicationSymbol.SafeGetBodyPropertyValue(LanguageConstants.ResourceNamePropertyName) ?? throw new ArgumentException($"Expected application syntax body to contain property 'name'");

            var expression = new StringSyntax(
                new []
                { 
                    new Token(TokenType.StringLeftPiece, new TextSpan(0, 0), "radius/", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                    new Token(TokenType.StringRightPiece, new TextSpan(0, 0), "", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                },
                new SyntaxBase[]
                {
                    nameValueSyntax,
                },
                new [] { "radius/", "", });
            return ConvertExpression(expression);
        }

        public LanguageExpression GetResourceNameExpression(ComponentSymbol componentSymbol)
        {
            var componentSyntax = componentSymbol.DeclaringComponent;
            if (!(componentSyntax.Body is ObjectSyntax objectSyntax))
            {
                // this condition should have already been validated by the type checker
                throw new InvalidOperationException($"Expected component syntax to have type {typeof(ObjectSyntax)}, but found {componentSyntax.Body.GetType()}");
            }

            if (componentSymbol.Parent == null)
            {
                var applicationPropertySyntax = objectSyntax.Properties.FirstOrDefault(p => LanguageConstants.IdentifierComparer.Equals(p.TryGetKeyText(), "application"));
                if (applicationPropertySyntax == null)
                {
                    // this condition should have already been validated by the type checker
                    throw new InvalidOperationException($"Expected component syntax body to contain property 'application'");
                }
                var applicationValue = applicationPropertySyntax.Value;

                var namePropertySyntax = objectSyntax.Properties.FirstOrDefault(p => LanguageConstants.IdentifierComparer.Equals(p.TryGetKeyText(), "name"));
                if (namePropertySyntax == null)
                {
                    // this condition should have already been validated by the type checker
                    throw new InvalidOperationException($"Expected component syntax body to contain property 'name'");
                }

                var expression = new StringSyntax(
                    new []
                    { 
                        new Token(TokenType.StringLeftPiece, new TextSpan(0, 0), "", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                        new Token(TokenType.StringMiddlePiece, new TextSpan(0, 0), "/", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                        new Token(TokenType.StringRightPiece, new TextSpan(0, 0), "", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                    },
                    new []
                    {
                        applicationValue,
                        namePropertySyntax.Value,
                    },
                    new [] { "", "/", "", });
                return ConvertExpression(expression);
            }
            else
            {
                if (!(componentSymbol.Parent.DeclaringApplication.Body is ObjectSyntax applicationBody))
                {
                    // this condition should have already been application by the type checker
                    throw new InvalidOperationException($"Expected application syntax to have type {typeof(ObjectSyntax)}, but found {componentSymbol.Parent.DeclaringApplication.Body.GetType()}");
                }

                var applicationPropertySyntax = applicationBody.Properties.FirstOrDefault(p => LanguageConstants.IdentifierComparer.Equals(p.TryGetKeyText(), "name"));
                if (applicationPropertySyntax == null)
                {
                    // this condition should have already been validated by the type checker
                    throw new InvalidOperationException($"Expected application syntax body to contain property 'name'");
                }

                var applicationValue = applicationPropertySyntax.Value;
                
                var namePropertySyntax = objectSyntax.Properties.FirstOrDefault(p => LanguageConstants.IdentifierComparer.Equals(p.TryGetKeyText(), "name"));
                if (namePropertySyntax == null)
                {
                    // this condition should have already been validated by the type checker
                    throw new InvalidOperationException($"Expected component syntax body to contain property 'name'");
                }

                var expression = new StringSyntax(
                    new []
                    { 
                        new Token(TokenType.StringLeftPiece, new TextSpan(0, 0), "radius/", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                        new Token(TokenType.StringMiddlePiece, new TextSpan(0, 0), "/", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                        new Token(TokenType.StringRightPiece, new TextSpan(0, 0), "", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                    },
                    new []
                    {
                        applicationValue,
                        namePropertySyntax.Value,
                    },
                    new [] { "radius/", "/", "", });
                return ConvertExpression(expression);
            }
        }

        public LanguageExpression GetResourceNameExpression(DeploymentSymbol deploymentSymbol)
        {
            var deploymentSyntax = deploymentSymbol.DeclaringDeployment;
            if (!(deploymentSyntax.Body is ObjectSyntax objectSyntax))
            {
                // this condition should have already been validated by the type checker
                throw new InvalidOperationException($"Expected component syntax to have type {typeof(ObjectSyntax)}, but found {deploymentSymbol.Body.GetType()}");
            }

            if (deploymentSymbol.Parent == null)
            {
                var applicationPropertySyntax = objectSyntax.Properties.FirstOrDefault(p => LanguageConstants.IdentifierComparer.Equals(p.TryGetKeyText(), "application"));
                if (applicationPropertySyntax == null)
                {
                    // this condition should have already been validated by the type checker
                    throw new InvalidOperationException($"Expected component syntax body to contain property 'application'");
                }
                var applicationValue = applicationPropertySyntax.Value;

                var namePropertySyntax = objectSyntax.Properties.FirstOrDefault(p => LanguageConstants.IdentifierComparer.Equals(p.TryGetKeyText(), "name"));
                if (namePropertySyntax == null)
                {
                    // this condition should have already been validated by the type checker
                    throw new InvalidOperationException($"Expected component syntax body to contain property 'name'");
                }

                var expression = new StringSyntax(
                    new []
                    { 
                        new Token(TokenType.StringLeftPiece, new TextSpan(0, 0), "", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                        new Token(TokenType.StringMiddlePiece, new TextSpan(0, 0), "/", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                        new Token(TokenType.StringRightPiece, new TextSpan(0, 0), "", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                    },
                    new []
                    {
                        applicationValue,
                        namePropertySyntax.Value,
                    },
                    new [] { "", "/", "", });
                return ConvertExpression(expression);
            }
            else
            {
                if (!(deploymentSymbol.Parent.DeclaringApplication.Body is ObjectSyntax applicationBody))
                {
                    // this condition should have already been application by the type checker
                    throw new InvalidOperationException($"Expected application syntax to have type {typeof(ObjectSyntax)}, but found {deploymentSymbol.Parent.DeclaringApplication.Body.GetType()}");
                }

                var applicationPropertySyntax = applicationBody.Properties.FirstOrDefault(p => LanguageConstants.IdentifierComparer.Equals(p.TryGetKeyText(), "name"));
                if (applicationPropertySyntax == null)
                {
                    // this condition should have already been validated by the type checker
                    throw new InvalidOperationException($"Expected application syntax body to contain property 'name'");
                }

                var applicationValue = applicationPropertySyntax.Value;
                
                var namePropertySyntax = objectSyntax.Properties.FirstOrDefault(p => LanguageConstants.IdentifierComparer.Equals(p.TryGetKeyText(), "name"));
                if (namePropertySyntax == null)
                {
                    // this condition should have already been validated by the type checker
                    throw new InvalidOperationException($"Expected component syntax body to contain property 'name'");
                }

                var expression = new StringSyntax(
                    new []
                    { 
                        new Token(TokenType.StringLeftPiece, new TextSpan(0, 0), "radius/", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                        new Token(TokenType.StringMiddlePiece, new TextSpan(0, 0), "/", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                        new Token(TokenType.StringRightPiece, new TextSpan(0, 0), "", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                    },
                    new []
                    {
                        applicationValue,
                        namePropertySyntax.Value,
                    },
                    new [] { "radius/", "/", "", });
                return ConvertExpression(expression);
            }
        }

        public FunctionExpression GetReferenceExpression(DeclaredSymbol symbol, bool full)
        {
            switch (symbol)
            {
                case ResourceSymbol resourceSymbol:
                {
                    var typeReference = EmitHelpers.GetTypeReference(resourceSymbol);
                    return GetReferenceExpression(resourceSymbol, typeReference, full);
                }

                case ApplicationSymbol applicationSymbol:
                {
                    var typeReference = EmitHelpers.GetTypeReference(applicationSymbol);
                    return GetReferenceExpression(applicationSymbol, typeReference, full);
                }

                case ComponentSymbol componentSymbol:
                {
                    var resourceName = GetResourceNameExpression(componentSymbol);
                    var typeReference = EmitHelpers.GetTypeReference(componentSymbol);
                    return GetReferenceExpression(componentSymbol, typeReference, full);
                }

                case DeploymentSymbol deploymentSymbol:
                {
                    var typeReference = EmitHelpers.GetTypeReference(deploymentSymbol);
                    return GetReferenceExpression(deploymentSymbol, typeReference, full);
                }

                default:
                    throw new InvalidOperationException($"Unexpected symbol type {symbol.GetType()}");
            }
        }

        public FunctionExpression GetReferenceExpression(ApplicationSymbol applicationSymbol, ResourceTypeReference typeReference, bool full)
        {
            // full gives access to top-level resource properties, but generates a longer statement
            if (full)
            {
                return CreateFunction(
                    "reference",
                    GetResourceIdExpression(GetResourceNameExpression(applicationSymbol), typeReference),
                    new JTokenExpression(typeReference.ApiVersion),
                    new JTokenExpression("full"));
            }

            return CreateFunction(
                "reference",
                GetResourceIdExpression(GetResourceNameExpression(applicationSymbol), typeReference));
        }

        public FunctionExpression GetReferenceExpression(ComponentSymbol componentSymbol, ResourceTypeReference typeReference, bool full)
        {
            // full gives access to top-level resource properties, but generates a longer statement
            if (full)
            {
                return CreateFunction(
                    "reference",
                    GetResourceIdExpression(GetResourceNameExpression(componentSymbol), typeReference),
                    new JTokenExpression(typeReference.ApiVersion),
                    new JTokenExpression("full"));
            }

            return CreateFunction(
                "reference",
                GetResourceIdExpression(GetResourceNameExpression(componentSymbol), typeReference));
        }

        public FunctionExpression GetReferenceExpression(DeploymentSymbol deploymentSymbol, ResourceTypeReference typeReference, bool full)
        {
            // full gives access to top-level resource properties, but generates a longer statement
            if (full)
            {
                return CreateFunction(
                    "reference",
                    GetResourceIdExpression(GetResourceNameExpression(deploymentSymbol), typeReference),
                    new JTokenExpression(typeReference.ApiVersion),
                    new JTokenExpression("full"));
            }

            return CreateFunction(
                "reference",
                GetResourceIdExpression(GetResourceNameExpression(deploymentSymbol), typeReference));
        }

        public FunctionExpression GetResourceIdExpression(ResourceReference resource)
        {
            var resourceName = GetResourceNameExpression(resource.Name);
            var typeReference = resource.ResourceType;
            return GetResourceIdExpression(resourceName, typeReference);
        }

        public FunctionExpression GetResourceIdExpression(DeclaredSymbol symbol)
        {
            switch (symbol)
            {
                case ResourceSymbol resourceSymbol:
                {
                    var resourceName = GetResourceNameExpression(resourceSymbol);
                    var typeReference = EmitHelpers.GetTypeReference(resourceSymbol);
                    return GetResourceIdExpression(resourceName, typeReference);
                }

                case ApplicationSymbol applicationSymbol:
                {
                    var resourceName = GetResourceNameExpression(applicationSymbol);
                    var typeReference = EmitHelpers.GetTypeReference(applicationSymbol);
                    return GetResourceIdExpression(resourceName, typeReference);
                }

                case ComponentSymbol componentSymbol:
                {
                    var resourceName = GetResourceNameExpression(componentSymbol);
                    var typeReference = EmitHelpers.GetTypeReference(componentSymbol);
                    return GetResourceIdExpression(resourceName, typeReference);
                }

                case DeploymentSymbol deploymentSymbol:
                {
                    var resourceName = GetResourceNameExpression(deploymentSymbol);
                    var typeReference = EmitHelpers.GetTypeReference(deploymentSymbol);
                    return GetResourceIdExpression(resourceName, typeReference);
                }

                default:
                    throw new InvalidOperationException($"Unexpected symbol type {symbol.GetType()}");
            }
        }

        public FunctionExpression GetResourceIdExpression(LanguageExpression resourceName, ResourceTypeReference typeReference)
        {
            if (typeReference.Types.Length == 1)
            {
                return new FunctionExpression(
                    "resourceId",
                    new LanguageExpression[]
                    {
                        new JTokenExpression(typeReference.FullyQualifiedType),
                        resourceName,
                    },
                    Array.Empty<LanguageExpression>());
            }

            var nameSegments = typeReference.Types.Select(
                (type, i) => new FunctionExpression(
                    "split",
                    new LanguageExpression[] { resourceName, new JTokenExpression("/") },
                    new LanguageExpression[] { new JTokenExpression(i) }));

            return new FunctionExpression(
                "resourceId",
                new LanguageExpression[]
                {
                    new JTokenExpression(typeReference.FullyQualifiedType),
                }.Concat(nameSegments).ToArray(),
                Array.Empty<LanguageExpression>());                    
        }

        private LanguageExpression ConvertString(CompoundName name)
        {
            if (name.Segments.Length == 1)
            {
                return ConvertString(name.Segments[0]);
            }

            var args = new LanguageExpression[name.Segments.Length + 1];
            args[0] = new JTokenExpression(string.Join("/", Enumerable.Range(0, name.Segments.Length).Select(i => $"{{{i}}}")));
            for (var i = 0; i < name.Segments.Length; i++)
            {
                args[i + 1] = ConvertString(name.Segments[i]);
            }

            return new FunctionExpression("format", args, Array.Empty<LanguageExpression>());
        }

        private LanguageExpression ConvertString(CompoundName.Segment segment)
        {
            if (segment.Expression is SyntaxBase syntax)
            {
                return ConvertExpression(syntax);
            }
            else if (segment.Literal is string literal)
            {
                return new JTokenExpression(literal);
            }

            throw new InvalidOperationException("Unreachable.");
        }
    }
}