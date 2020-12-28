using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ZptSharp
{
    [TestFixture,Parallelizable]
    public class StreamToTextWriterCopierTests
    {
        #region sample stream content

        /// <summary>
        /// This is just a long sample string which will make up our stream.
        /// </summary>
        const string streamContent = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus sem magna, elementum quis arcu sed, dictum lacinia mi. Aliquam erat volutpat. Nulla ac velit urna. Suspendisse euismod magna sed augue lobortis rhoncus. Aenean ipsum sapien, blandit at purus ac, consectetur vehicula nibh. Quisque sollicitudin ac tortor nec pellentesque. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Vestibulum in lorem non nunc facilisis pretium non varius mauris.

Nunc gravida, purus eu gravida posuere, nisi tellus tincidunt libero, id egestas leo sem non sapien. Proin sit amet iaculis est, sit amet tristique est. Praesent pharetra est at augue imperdiet, et sollicitudin nibh varius. Donec eu tristique massa. Donec vitae fermentum leo, sit amet fringilla lectus. In vel dapibus ante. Sed nunc nibh, ullamcorper sit amet lacinia in, sollicitudin vitae metus. Cras at risus orci. Praesent vehicula leo rutrum nisi luctus suscipit. Fusce posuere dapibus sem a auctor. Nulla et sodales dolor. Mauris eu rhoncus metus, a sagittis nibh. Aenean sed ex nec metus cursus condimentum id vel lacus. Nulla viverra feugiat ante, viverra laoreet sem tempus at. Ut mollis, lacus id consequat tristique, sapien diam accumsan ex, vel mattis enim sapien imperdiet ante.

Vivamus ultricies fringilla dui, non convallis turpis venenatis ac. Vestibulum ante urna, imperdiet in dictum nec, iaculis vitae quam. Quisque dictum tellus ex, ac congue est dignissim vitae. Nullam eu rutrum dolor, id varius nisl. Sed sodales, dolor quis dictum lacinia, erat nibh pellentesque leo, in finibus velit ligula ut eros. Quisque imperdiet diam nec faucibus rhoncus. Ut cursus, nisi nec sollicitudin egestas, justo metus ultricies ex, eget interdum ipsum ante quis est. Cras finibus pretium placerat. Nulla venenatis, ante sed gravida euismod, odio urna commodo velit, sit amet condimentum ante enim ac felis. In suscipit sit amet mauris at malesuada.

Duis ornare, tortor sit amet viverra accumsan, nisl turpis suscipit tortor, ac euismod odio dui ac lacus. Nunc dictum, enim in dignissim tincidunt, lectus erat vulputate ipsum, sit amet venenatis nisi turpis id justo. Aliquam gravida felis et felis fringilla, et facilisis diam cursus. Quisque sit amet ullamcorper ex. Praesent varius quam ut sapien interdum, hendrerit congue tellus convallis. Aliquam eget libero nec lacus tempus euismod auctor vel sem. Nullam eu rhoncus diam.

Curabitur sit amet convallis velit. Nunc porta pharetra libero vitae posuere. Sed sodales sagittis tellus, sit amet malesuada metus pellentesque tempus. Cras malesuada nibh risus, nec dignissim nunc pharetra non. Ut ac tellus vestibulum, feugiat arcu ut, bibendum leo. Nullam nunc sem, tincidunt sit amet accumsan fermentum, aliquam non felis. Aliquam erat volutpat. Integer vestibulum ipsum efficitur nisl vestibulum, quis lobortis odio mollis. Mauris sagittis dui pulvinar odio semper pharetra. Vestibulum vel maximus massa. Cras non odio lorem. Aliquam non ultricies felis. Nunc tempus, lorem vitae pulvinar consequat, nisl odio mattis nunc, ut lacinia nisl nulla vitae mauris. Phasellus blandit ante vitae lectus dictum varius.

Vivamus vitae mollis diam. Pellentesque ullamcorper nibh magna, vitae vestibulum tellus aliquet nec. Curabitur vel consequat libero, eu suscipit enim. Nunc facilisis tellus quis augue tristique consectetur. Nulla et nisi hendrerit, interdum tellus laoreet, congue tellus. Nam vestibulum id odio sed blandit. Ut semper, dui in pretium lobortis, eros dui viverra orci, et faucibus arcu nisi quis sem. Donec semper ante neque, sagittis consequat velit vehicula quis. Donec pretium turpis vel sapien ultricies fermentum. Aliquam dictum leo eros, vitae sagittis ipsum ullamcorper sit amet.

Aenean id vulputate sem. Integer facilisis lacinia urna, aliquet accumsan sem viverra sed. Sed lobortis sem risus, quis pellentesque est tincidunt non. Nam vel malesuada urna. Sed luctus fermentum pulvinar. In diam nulla, sollicitudin at dolor ac, placerat hendrerit lorem. In viverra, est id eleifend varius, mauris mi molestie enim, sed dapibus dolor orci nec purus. Quisque laoreet, enim vitae venenatis tempus, purus ligula accumsan ex, eleifend faucibus tortor dui ut leo. Fusce urna urna, pretium ac tellus in, faucibus accumsan metus. Nullam id dapibus ex, quis rhoncus dolor. Vestibulum sit amet lorem dictum, feugiat lacus id, congue nunc. Nunc ex nisi, auctor ut massa nec, condimentum scelerisque purus. Vivamus ac commodo lorem.

Curabitur volutpat ex sit amet nisi dignissim sollicitudin. Sed non elit eu est varius hendrerit. Nullam turpis sem, sagittis vel diam vitae, tempus pulvinar massa. Sed non tincidunt ipsum. In vel enim eget nisi aliquam hendrerit ornare nec ante. Sed mattis metus dapibus volutpat finibus. Nam vitae ligula ipsum. Vivamus dignissim tortor eget elementum ultrices. Vestibulum odio est, posuere sit amet aliquam vitae, tristique fermentum ligula. Sed sed rhoncus sapien, ac pulvinar purus. Ut varius ex tempor, tristique velit bibendum, ultrices quam.

Mauris non efficitur metus. Curabitur tristique ante id lacus gravida, ut rhoncus ante dapibus. Vivamus semper sem magna, ac tincidunt nulla dignissim eget. Nulla molestie nisi sem, non gravida arcu euismod viverra. Etiam venenatis nisl eu nibh volutpat vulputate. Nunc quis ligula eros. Proin lacus dui, elementum vel leo in, gravida cursus ex. Cras lacinia nibh aliquet eros vulputate iaculis. Sed malesuada lacus accumsan enim pharetra, a eleifend dui faucibus. Quisque condimentum sed tortor et accumsan. Nunc sit amet aliquam neque, interdum molestie nibh. Mauris vulputate congue enim, vitae ullamcorper tellus ornare nec. Vivamus sit amet justo ac ipsum pulvinar varius. Nam porta mauris dictum nibh ullamcorper, sed vehicula nisi blandit. In eget ipsum ut odio maximus porttitor eget nec lorem. Donec suscipit, arcu volutpat fringilla euismod, tortor ex dignissim ligula, vel pharetra eros dolor sit amet nisi.

Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Phasellus id pretium lorem. Nullam vehicula elementum aliquet. Vestibulum mi tellus, varius vitae diam sit amet, condimentum faucibus libero. Sed in dolor neque. Nullam nunc velit, malesuada sed faucibus eu, porttitor ac mauris. Etiam molestie non magna tincidunt bibendum. Mauris a tellus magna. Pellentesque ut velit egestas, placerat orci nec, viverra ipsum. Maecenas iaculis lobortis aliquam. Pellentesque laoreet vehicula mauris, vitae fermentum nisl sodales vitae. Etiam vehicula ante quis eleifend interdum.";

        #endregion

        [Test, AutoMoqData]
        public async Task WriteToTextWriterAsync_can_write_to_a_StreamWriter(StreamToTextWriterCopier sut)
        {
            var sourceStream = GetStream();
            var destStream = new MemoryStream();

            using (var writer = new StreamWriter(destStream, leaveOpen: true))
            {
                await sut.WriteToTextWriterAsync(sourceStream, writer);
            }

            destStream.Position = 0;

            using (var reader = new StreamReader(destStream))
            {
                Assert.That(() => reader.ReadToEnd(), Is.EqualTo(streamContent));
            }
        }

        [Test, AutoMoqData]
        public async Task WriteToTextWriterAsync_can_write_to_a_StringWriter(StreamToTextWriterCopier sut)
        {
            var sourceStream = GetStream();
            var stringBuilder = new StringBuilder();

            using (var writer = new StringWriter(stringBuilder))
            {
                await sut.WriteToTextWriterAsync(sourceStream, writer);
            }

            Assert.That(() => stringBuilder.ToString(), Is.EqualTo(streamContent));
        }

        Stream GetStream()
        {
            var stream = new MemoryStream();

            using (var writer = new StreamWriter(stream, leaveOpen: true))
            {
                writer.Write(streamContent);
                writer.Flush();
            }

            stream.Position = 0;
            return stream;
        }
    }
}
