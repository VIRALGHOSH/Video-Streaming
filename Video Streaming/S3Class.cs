using Amazon.Auth;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Video_Streaming
{
    public class S3Class
    {
        AmazonS3Client client = new AmazonS3Client("AKIAI7S3DS2CRLQCQWOQ", "gkx0djo5+8gXh8qxxuEmdY833f/Kij/ZsvbGyXgm", Amazon.RegionEndpoint.USWest2);
        
        public string putObject(string bucketName, string filepath, string keyName)
        {
            try
            {
                  PutObjectRequest putRequest1 = new PutObjectRequest
                  {
                      BucketName = bucketName,
                      Key = keyName,
                      FilePath = filepath,
                      CannedACL  = S3CannedACL.PublicReadWrite
                  };
                  PutObjectResponse response1 = client.PutObject(putRequest1);

                  if (bucketName == "transcoder.input.video.streaming")
                  {
                      string fileName = keyName;
                      int fileExtPos = fileName.LastIndexOf(".");
                      if (fileExtPos >= 0)
                          fileName = fileName.Substring(0, fileExtPos);

                      return "https://d2vurfo3zy67rq.cloudfront.net/" + fileName + ".m3u8";
                  }
                   if (bucketName == "transcoder.thumbnail.video.streaming")
                   {
                       return "https://d3n8dmuoyz8caz.cloudfront.net/" + keyName;
                   }
                   else
                   {
                       return "https://d2tn3apwhreka0.cloudfront.net/" + keyName;
                   }
            }
            catch (Exception exc){
                throw exc;
            }
        }
    }
} 