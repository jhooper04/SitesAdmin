pipeline {
    agent any
    stages {
        stage("Build") {
            steps {
                withCredentials([file(credentialsId: 'sitesadmin-env', variable: 'ENV_PATH')]) {
                    sh '''
                        cp $ENV_PATH .env.production
                        docker compose -f docker-compose.yml -f docker-compose.production.yml --env-file .env.production build
                    '''
                }
            }
        }
        stage("Deploy") {
            steps {
                withCredentials([file(credentialsId: 'sitesadmin-env', variable: 'ENV_PATH')]) {
                    sh '''
                        if [ $(docker ps -q -f name=sitesadmin) ]; then
                            docker compose -f docker-compose.yml -f docker-compose.production.yml down
                        fi
                        cp $ENV_PATH .env.production
                        docker compose -f docker-compose.yml -f docker-compose.production.yml --env-file .env.production up -d
                    '''
                }
            }
        }
    }
}