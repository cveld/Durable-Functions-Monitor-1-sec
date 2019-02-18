Kick off the durable function through PostMan with the following url:

http://localhost:7071/api/orchestrators/MonitorTest
With a JSON body payload:
{
	"duration": 1000
}

You will notice that locally the function gets looped 10 times or so.
But in the cloud you will notice that the function gets looped almost 100 times!

As soon as you increase the duration a bit this behavior is gone:
{
	"duration": 1050
}
