using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace testeCRUD2
{
    public partial class Form1 : Form
    {
        private MySqlConnection Conexao;
        private string data_source = "datasource=localhost;username=root;password=1234;database=db_agenda2";
        public Form1()
        {
            InitializeComponent();
            listarContatos();
        }

        private void listarContatos()
        {
            Conexao = new MySqlConnection(data_source);
            Conexao.Open();

            string query = "SELECT * FROM contato ORDER BY id desc";//show latest added first

            MySqlCommand cmd = new MySqlCommand(query, Conexao);

            cmd.Parameters.Clear();
            
            lstContatos.Items.Clear();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string[] row =
                {
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                };
                lstContatos.Items.Add(new ListViewItem(row));
            }
            Conexao.Close();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "")
            {
                MessageBox.Show("Preencha o nome");
                return;
            }
            else if (txtTelefone.Text == "")
            {
                MessageBox.Show("Preencha o telefone");
                return;
            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Preencha o email");
                return;
            }else
            {
                try
                {
                
                    //Open connection using MySql documentation partten
                    Conexao = new MySqlConnection(data_source);
                    Conexao.Open();

                    string query = "INSERT INTO contato VALUES (NULL, @nome, @telefone, @email)";//Comando.CommandText

                    //set comands
                    MySqlCommand Comando;
                    Comando = new MySqlCommand(query, Conexao);// receive values of (Comando.CommandText, Comando.Connection)
                    Comando.Parameters.Clear();
                    Comando.Parameters.AddWithValue("@nome", txtNome.Text);
                    Comando.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    Comando.Parameters.AddWithValue("@email", txtEmail.Text);
                    Comando.ExecuteNonQuery();

                    MessageBox.Show("Contato salvo!");

            
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Conexao.Close();
                }
                txtNome.Clear();
                txtTelefone.Clear();
                txtEmail.Clear();
                listarContatos();
            }

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Conexao = new MySqlConnection(data_source);
                Conexao.Open();

                string sql = "select * from contato where nome LIKE @query OR email LIKE @query";

                MySqlCommand comando = new MySqlCommand(sql, Conexao);

                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@query", "%" + txtBuscar.Text + "%");
                
                MySqlDataReader reader = comando.ExecuteReader();

                lstContatos.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                    };

                    lstContatos.Items.Add(new ListViewItem(row));
                }

                MessageBox.Show("Pesquisa concluida!");

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }finally
            {
                Conexao.Close();
            }
        }
    }
}
