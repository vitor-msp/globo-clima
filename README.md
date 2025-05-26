# Globo Clima

Foi desenvolvida uma api para consulta de dados climáticos de um endereço e dados demográficos de um país, com opções de favoritar.
A api foi construída em .Net 8.0, TDD usando xUnit, autenticação com JWT e documentada com Swagger.
A infraestrutura foi implementada na AWS com o CloudFormation, usando o DynamoDB para armazenamento, ALB e ECS para escalabilidade, Code Pipeline para CI/CD e CloudWatch para observabilidade.

### Coverage dos testes na api
![print do coverage](assets/coverage.png)

### Diagrama da infraestrutura na AWS
![infraestrutura AWS](assets/aws-infrastructure.png)

### Rede virtual (VPC)
![print da VPC](assets/vpc.png)

### Load Balancer configurado com HTTPS
![print do ALB](assets/load-balancer.png)

### Tasks rodando no ECS para escalabilidade
![print das tasks do ECS](assets/ecs-tasks.png)

### Dados armazenados no DynamoDB com senhas criptografadas
![print do DynamoDB](assets/dynamodb.png)

### Métricas dos conteiners no CloudWatch
![print das métricas do ECS](assets/ecs-metrics.png)

### Logs gerados pela api centralizados no CloudWatch
![print dos logs do ECS](assets/ecs-logs.png)

### Pipeline CI/CD no CodePipeline
![print da pipeline](assets/pipeline.png)

### Imagens Docker armazenadas no ECR
![print do ECR](assets/ecr.png)