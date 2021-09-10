# What is done
1. Redis caching. 
    - added data to redis happens in the Traiting2.BackgroundService.UpdateRedisService
    - there is one endpoint (https://localhost:5001/api/Announcement/GetMostPopularAnnouncement) reading data from redis
    - command for docker "docker run --name my-redis -p 5002:6379 -d redis"
2. Work with MongoDB
    - ArticleController work with MongoDB
3. ProductPhotoController has optimization for images
    - resizing image uses "ImageMagick" library
    - compressing image uses "libvips" library
4. Watch streaming video
    - implemented in "VideoStreamController"
    - test html be in the "TestHTML" folder
5. Listen streaming audio
    - implemented in "AudioStreamController"
    - test html be in the "TestHTML" folder