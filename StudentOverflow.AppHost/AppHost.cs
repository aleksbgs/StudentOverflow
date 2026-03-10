var builder = DistributedApplication.CreateBuilder(args);


#pragma warning disable ASPIRECERTIFICATES001

var keycloak = builder.AddKeycloak("keycloak", 6001)

    .WithoutHttpsCertificate()

    .WithDataVolume("keycloak-data");


var postgres = builder.AddPostgres("postgres", port: 5432)
    .WithDataVolume("postgres-data")
    .WithPgAdmin();

var questionDb = postgres.AddDatabase("questionDb");


var questionService = builder.AddProject<Projects.QuestionService>("QuestionService")
    .WithReference(keycloak)
    .WithReference(questionDb)
    .WaitFor(questionDb)
    .WaitFor(keycloak);


#pragma warning restore ASPIRECERTIFICATES001

builder.Build().Run();