using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using LZOHelper;

namespace Zlibber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.*|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] buff = File.ReadAllBytes(d.FileName);
                hb1.ByteProvider = new DynamicByteProvider(buff);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.*|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] buff = File.ReadAllBytes(d.FileName);
                hb2.ByteProvider = new DynamicByteProvider(buff);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = "*.*|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MemoryStream m = new MemoryStream();
                for (long i = 0; i < hb1.ByteProvider.Length; i++)
                    m.WriteByte(hb1.ByteProvider.ReadByte(i));
                File.WriteAllBytes(d.FileName, m.ToArray());
                MessageBox.Show("Done.");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = "*.*|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MemoryStream m = new MemoryStream();
                for (long i = 0; i < hb2.ByteProvider.Length; i++)
                    m.WriteByte(hb2.ByteProvider.ReadByte(i));
                File.WriteAllBytes(d.FileName, m.ToArray());
                MessageBox.Show("Done.");
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            MemoryStream m = new MemoryStream();
            for (long i = 0; i < hb2.ByteProvider.Length; i++)
                m.WriteByte(hb2.ByteProvider.ReadByte(i));
            hb1.ByteProvider = new DynamicByteProvider(CompressZlib(m.ToArray()));
        }

        public static byte[] CompressLZO(byte[] input)
        {            
            return LZOCompressor.Compress(input);
        }
        public static byte[] DecompressLZO(byte[] input)
        {
            return LZOCompressor.Decompress(input);
        }


        public static byte[] DecompressZlib(byte[] input)
        {
            MemoryStream source = new MemoryStream(input);
            byte[] result = null;
            using (MemoryStream outStream = new MemoryStream())
            {
                using (InflaterInputStream inf = new InflaterInputStream(source))
                {
                    inf.CopyTo(outStream);
                }
                result = outStream.ToArray();
            }
            return result;
        }

        public static byte[] CompressZlib(byte[] input)
        {
            MemoryStream m = new MemoryStream();
            DeflaterOutputStream zipStream = new DeflaterOutputStream(m, new ICSharpCode.SharpZipLib.Zip.Compression.Deflater(8));
            zipStream.Write(input, 0, input.Length);
            zipStream.Finish();
            return m.ToArray();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            MemoryStream m = new MemoryStream();
            for (long i = 0; i < hb1.ByteProvider.Length; i++)
                m.WriteByte(hb1.ByteProvider.ReadByte(i));
            hb2.ByteProvider = new DynamicByteProvider(DecompressZlib(m.ToArray()));
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            MemoryStream m = new MemoryStream();
            for (long i = 0; i < hb2.ByteProvider.Length; i++)
                m.WriteByte(hb2.ByteProvider.ReadByte(i));
            hb1.ByteProvider = new DynamicByteProvider(CompressLZO(m.ToArray()));
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            MemoryStream m = new MemoryStream();
            for (long i = 0; i < hb1.ByteProvider.Length; i++)
                m.WriteByte(hb1.ByteProvider.ReadByte(i));
            hb2.ByteProvider = new DynamicByteProvider(DecompressLZO(m.ToArray()));
        }
    }
}
