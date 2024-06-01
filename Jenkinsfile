pipeline {
    agent any
    stages {
        stage("Build") {
            steps {
                withCredentials([file(credentialsId: 'sitesadmin-env', variable: 'ENV_PATH')]) {
                    sh '''
                        cp $ENV_PATH .env.production
                        docker compose up --build -f docker-compose.yml --env-file .env.production 
                    '''
                }
            }
        }
        stage("Deploy") {
            steps {
                withCredentials([file(credentialsId: 'sitesadmin-env', variable: 'ENV_PATH')]) {
                    sh '''
                        if [ $(docker ps -q -f name=sitesadmin) ]; then
                            docker compose down -f docker-compose.yml
                        fi
                        cp $ENV_PATH .env.production
                        docker compose up -f docker-compose.yml --env-file .env.production 
                    '''
                }
            }
        }
    }
}