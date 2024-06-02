pipeline {
    agent any
    stages {
        stage("Build") {
            steps {
                withCredentials([file(credentialsId: 'sitesadmin-env', variable: 'ENV_PATH')]) {
                    sh '''
                        cp $ENV_PATH .env.production
                        docker compose -f docker-compose.yml -f docker-compose.production.yml --env-file .env.production build

                        # Get the list of image names and their corresponding build numbers
                        images=$(docker images --format "{{.Repository}}:{{.Tag}}" | grep 'sitesadmin-' | sort -t'-' -k2 -n)

                        # Get the last two image names
                        last_two_images=$(echo "$images" | tail -n 2)

                        # Iterate over the images and remove the old ones
                        echo "$images" | while read -r image_name; do
                          if ! echo "$last_two_images" | grep -q "$image_name"; then
                            echo "Removing image: $image_name"
                            docker rmi -f "$image_name"
                          else 
                            echo "Keeping image: $image_name"
                          fi
                        done
                    '''
                }
            }
        }
        stage("Deploy") {
            steps {
                withCredentials([file(credentialsId: 'sitesadmin-env', variable: 'ENV_PATH')]) {
                    sh '''
                        if [ $(docker ps -q -f name=sitesadmin) ]; then
                            docker compose -f docker-compose.yml -f docker-compose.production.yml --env-file .env.production down
                        fi
                        cp $ENV_PATH .env.production
                        docker compose -f docker-compose.yml -f docker-compose.production.yml --env-file .env.production up -d
                    '''
                }
            }
        }
    }
}