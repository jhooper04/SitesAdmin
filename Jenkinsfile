pipeline {
    agent any
    stages {
        stage("Build") {
            steps {
                withCredentials([file(credentialsId: 'sitesadmin-env', variable: 'ENV_PATH')]) {
                    sh '''
                        docker compose up -f docker-compose.yml up --build 
                    '''
                }
            }
        }
        stage("Deploy") {
            steps {
                withCredentials([file(credentialsId: 'sitesadmin-env', variable: 'ENV_PATH')]) {
                    sh '''
                        if [ $(docker ps -q -f name=sitesadmin) ]; then
                            docker compose up -f docker-compose.yml down
                        fi
                        docker compose up -f docker-compose.yml up --no-build
                    '''
                }
            }
        }
    }
}