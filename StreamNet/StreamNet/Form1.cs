using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;

using Org.BouncyCastle.Crypto.Digests;

using System.IO;
using Org.BouncyCastle.Crypto;

namespace StreamNet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            thread1 = new Thread(new ThreadStart(des));
            thread2 = new Thread(new ThreadStart(snefru));
            thread3 = new Thread(new ThreadStart(backpack));
        }

        Thread thread1;
        Thread thread2;
        Thread thread3;

        private void des()
        {
            try
            {
                // Get the plaintext and key from the input boxes
                string plaintext = richTextBox1.Text;
                string key = "01234567";

                // Convert the plaintext and key to byte arrays
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);

                // Create a new instance of the DES algorithm with the key
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = keyBytes;

                // Create a new MemoryStream to hold the encrypted data
                MemoryStream ms = new MemoryStream();

                // Create a new CryptoStream to encrypt the data
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                // Write the plaintext to the CryptoStream
                cs.Write(plaintextBytes, 0, plaintextBytes.Length);

                // Flush the final block of data through the CryptoStream
                cs.FlushFinalBlock();

                // Convert the encrypted data to a Base64 string and output it to the result box
                string encryptedString = Convert.ToBase64String(ms.ToArray());

                // Output the encrypted text to RichTextBox2 (on the UI thread)
                Invoke(new Action(() => richTextBox4.Text = encryptedString));
            }
            catch (ThreadAbortException)
            {
                // Thread was aborted, do nothing
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void snefru()
        {
            try
            {
                // Get the plaintext from the input box
                string plaintext = richTextBox2.Text;

                // Convert the plaintext to a byte array
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

                // Create a new SHA-256 digest
                SHA256 sha256 = SHA256.Create();

                // Compute the first digest
                byte[] digest1 = sha256.ComputeHash(plaintextBytes);

                // Compute the second digest by hashing the first digest
                byte[] digest2 = sha256.ComputeHash(digest1);

                // Concatenate the two digests
                byte[] combinedDigest = new byte[digest1.Length + digest2.Length];
                Buffer.BlockCopy(digest1, 0, combinedDigest, 0, digest1.Length);
                Buffer.BlockCopy(digest2, 0, combinedDigest, digest1.Length, digest2.Length);

                // Compute the final digest by hashing the concatenated digest
                byte[] finalDigest = sha256.ComputeHash(combinedDigest);

                // Convert the final digest to a Base64 string and output it to the result box
                string encryptedString = Convert.ToBase64String(finalDigest);

                // Output the encrypted text to RichTextBox3 (on the UI thread)
                Invoke(new Action(() => richTextBox5.Text = encryptedString));
            }
            catch (ThreadAbortException)
            {
                // Thread was aborted, do nothing
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backpack()
        {

            try
            {
                // Get the plaintext from the input box
                string plaintext = richTextBox3.Text;

                // Define the public and private keys
                int[] publicKey = { 2, 3, 7, 14, 30, 57, 120, 251 };
                int privateKey = 23;

                // Convert the plaintext to binary
                string binaryString = string.Join("", Encoding.ASCII.GetBytes(plaintext).Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

                // Ensure that binaryString has the same length as publicKey
                if (binaryString.Length < publicKey.Length)
                {
                    binaryString = binaryString.PadRight(publicKey.Length, '0');
                }

                // Calculate the sum of the binary string multiplied by the public key
                long sum = 0;
                for (int i = 0; i < publicKey.Length; i++)
                {
                    if (binaryString[i] == '1')
                    {
                        sum += publicKey[i];
                    }
                };


                // Encrypt the sum using the private key
                long encryptedSum = (sum * privateKey) % 499;

                // Output the encrypted text to RichTextBox4 (on the UI thread)
                Invoke(new Action(() => richTextBox6.Text = encryptedSum.ToString()));
            }
            catch (ThreadAbortException)
            {
                // Thread was aborted, do nothing
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            thread1.Start();
            //draw_rect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            thread2.Start();
            //draw_eclips();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            thread3.Start();
            //Rnd_num();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            thread1.Suspend();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            thread2.Suspend();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            thread3.Suspend();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            thread1.Start();
            thread2.Start();
            thread3.Start();
            //draw_eclips();
            //draw_rect();
            //Rnd_num();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            thread1.Suspend();
            thread2.Suspend();
            thread3.Suspend();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            thread1.Abort();
            thread2.Abort();
            thread3.Abort();
        }

        
    }

    
}

