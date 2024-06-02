pipeline {
    agent any
    stages {
        stage("Build") {
            steps {
                withCredentials([file(credentialsId: 'sitesadmin-env', variable: 'ENV_PATH')]) {
                    sh '''
                        cp $ENV_PATH .env.production
                        docker compose -f docker-compose.yml -f docker-compose.production.yml --env-file .env.production build

                        # Get the list of image IDs and their corresponding build numbers
                        images=$(docker images --format "{{.ID}} {{.Repository}}" | grep 'sitesadmin-' | sort -t'-' -k2 -n)

                        # Get the last two image IDs
                        last_two_images=$(echo "$images" | tail -n 2 | awk '{print $1}')

                        # Iterate over the images and remove the old ones
                        echo "$images" | while read -r line; do
                          image_id=$(echo "$line" | awk '{print $1}')
                          if ! echo "$last_two_images" | grep -q "$image_id"; then
                            echo "Removing image: $image_id"
                            docker rmi "$image_id"
                          else
                            echo "Keeping image: $image_id"
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