//using Docker.DotNet;
//using Docker.DotNet.Models;





//namespace AuthenticateService.API.Common
//{
//    public class DockerService
//    {
//       private readonly DockerClient _dockerClient;

//        public DockerService()
//        {
//            // Khởi tạo Docker client
//            _dockerClient = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
//        }

//        public async Task<string> CompileAndRunTests(string sourceCode)
//        {
//            try
//            {
//                // Tạo một container mới từ Docker image có chứa trình biên dịch và các dependencies
//                var containerId = await CreateContainer();

//                // Copy mã nguồn vào container
//                await CopySourceCodeToContainer(containerId, sourceCode);
//                // Chạy các testcase trong container
//                var testResult = await RunTests(containerId);

//                // Xóa container sau khi hoàn thành
//                await RemoveContainer(containerId);

//                // Kết hợp kết quả biên dịch và kết quả các testcase để trả về kết quả tổng thể
//                var result = $"Compilation Result: {compilationResult}\nTest Result: {testResult}";

//                return result;
//            }
//            catch (Exception ex)
//            {
//                return $"An error occurred: {ex.Message}";
//            }
//        }

//        private async Task<string> CreateContainer()
//        {
//            var response = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
//            {
//                Image = "Compile", // Thay thế bằng Docker image chứa trình biên dịch và các dependencies
//                Cmd = new[] { "bash" } // Lệnh mặc định khi container được khởi chạy
//            });

//            return response.ID;
//        }

//        private async Task CopySourceCodeToContainer(string containerId, string sourceCode)
//        {
//            var tarStream = new MemoryStream();
//            using (var tar = new TarOutputStream(tarStream))
//            {
//                var bytes = System.Text.Encoding.UTF8.GetBytes(sourceCode);
//                var entry = new TarEntry("source.cs")
//                {
//                    Size = bytes.Length,
//                    //Mode = 0644,
//                };
//                tar.PutNextEntry(entry);
//                tar.Write(bytes, 0, bytes.Length);
//                tar.CloseEntry();
//            }

//            tarStream.Seek(0, SeekOrigin.Begin);
//            await _dockerClient.Containers.CopyToContainerAsync(containerId, "/", tarStream, new ContainerCopyOptions());
//        }

//        private async Task<string> CompileSourceCode(string containerId)
//        {
//            // Thực hiện lệnh biên dịch trong container
//            var execCreateParams = new ContainerExecCreateParameters
//            {
//                AttachStdout = true,
//                AttachStderr = true,
//                Cmd = new[] { "csc", "source.cs" }, // Lệnh để biên dịch source.cs
//                Tty = true,
//            };

//            var execCreateResult = await _dockerClient.Containers.ExecCreateContainerAsync(containerId, execCreateParams);
//            var execStartParams = new ContainerExecStartParameters
//            {
//                Detach = false,
//                Tty = true
//            };

//            using (var stream = await _dockerClient.Containers.StartContainerExecAsync(execCreateResult.ID, false, execStartParams))
//            {
//                var output = await stream.ReadOutputToEndAsync();
//                return output;
//            }
//        }

//        private async Task<string> RunTests(string containerId)
//        {
//            // Thực hiện lệnh chạy các testcase trong container
//            // Cung cấp mã logic để chạy các testcase ở đây
//            return "Test results";
//        }

//        private async Task RemoveContainer(string containerId)
//        {
//            // Xóa container sau khi hoàn thành
//            await _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());
//        }
//    }
//}
