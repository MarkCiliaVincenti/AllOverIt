﻿using AllOverIt.Aws.Cdk.AppSync.Factories;
using AllOverIt.Aws.Cdk.AppSync.Mapping;
using Amazon.CDK;
using Amazon.CDK.AWS.AppSync;
using GraphqlSchema.Schema;
using GraphqlSchema.Schema.Mappings;
using GraphqlSchema.Schema.Mappings.Query;
using System;

namespace GraphqlSchema.Constructs
{
    internal sealed class AppSyncConstruct : Construct
    {
        public AppSyncConstruct(Construct scope, AppSyncDemoAppProps appProps, AuthorizationMode authMode)
            : base(scope, "AppSync")
        {
            // This demo is using the [RequestResponseMapping] attributes to define all mappings, but they can alternatively be coded
            // using either of the two approaches below, and pass the 'mappingTemplates' to the AppSyncDemoGraphql instance, which can
            // then be passed to the base AppGraphqlBase class.

            //var mappingTemplates = new MappingTemplates();

            //mappingTemplates.RegisterMappings("Query.Continents", GetNoneRequestMapping(), GetNoneResponseMapping());
            //mappingTemplates.RegisterMappings("Query.Continents.Countries", GetHttpRequestMapping("GET", "/countries"), GetHttpResponseMapping());
            //mappingTemplates.RegisterMappings("Query.Continents.CountryCodes", GetHttpRequestMapping("GET", "/countryCodes"), GetHttpResponseMapping());
            //mappingTemplates.RegisterMappings("Query.AllContinents", GetHttpRequestMapping("GET", "/continents"), GetHttpResponseMapping());


            //mappingTemplates.RegisterQueryMappings(
            //    Mapping.Template(
            //        nameof(IAppSyncDemoQueryDefinition.Continents), GetNoneRequestMapping(), GetNoneResponseMapping(),
            //        new[]
            //        {
            //                  Mapping.Template(
            //                      nameof(IContinent.Countries), GetHttpRequestMapping("GET", "/countries"), GetHttpResponseMapping()),

            //                  Mapping.Template(
            //                      nameof(IContinent.CountryCodes), GetHttpRequestMapping("GET", "/countryCodes"), GetHttpResponseMapping())
            //        }),

            //    Mapping.Template(
            //        nameof(IAppSyncDemoQueryDefinition.AllContinents), GetHttpRequestMapping("GET", "/continents"), GetHttpResponseMapping())
            //    );


            // Providing the mapping for IAppSyncDemoQueryDefinition.CountryLanguage manually via code
            var mappingTemplates = new MappingTemplates();

            var noneMapping = new CountryLanguageMapping();     // using this just for convenience
            mappingTemplates.RegisterMappings("Query.CountryLanguage", noneMapping.RequestMapping, noneMapping.ResponseMapping);

            // Registering mapping types that don't have a default constructor (so runtime arguments can be provided)
            var mappingTypeFactory = new MappingTypeFactory();
            mappingTypeFactory.Register<ContinentLanguagesMapping>(() => new ContinentLanguagesMapping(true));

            // Based on a base class type
            mappingTypeFactory.Register<HttpGetResponseMappingBase>(type => (IRequestResponseMapping)Activator.CreateInstance(type, "super_secret_api_key" ));

            var graphql = new AppSyncDemoGraphql(this, appProps, authMode, mappingTemplates, mappingTypeFactory);

            graphql
                .AddSchemaQuery<IAppSyncDemoQueryDefinition>()
                .AddSchemaMutation<IAppSyncDemoMutationDefinition>()
                .AddSchemaSubscription<IAppSyncDemoSubscriptionDefinition>();
        }
    }
}