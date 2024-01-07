To test this project you will need a RabbitMX server running.
You can use the docker image provided by RabbitMQ.
You'll need to have Docker Desktop installed and running on the computer. 
Then just run the command below to create a container with RabbitMQ running on it:

docker run -d --hostname rmq --name rabbit-server -p 8080:15672 -p 5672:5672 rabbitmq:3-management



