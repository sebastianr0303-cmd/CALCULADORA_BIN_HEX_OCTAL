using System;
using System.Windows.Forms;

namespace Introduccion
{
    public partial class Form1 : Form
    {
        // Variables originales.
        private double N1 = 0;
        private double N2 = 0;
        private double resultado = 0;
        private bool op = false;
        private string operacion = "";

        // Tres indicadores para controlar el estado de la calculadora.
        private bool N2Ingresado = false;
        private bool error = false;
        private bool fin = false;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "0";
            textBox1.ReadOnly = true; // Los numeros se introducen con los botones.
            Text = "Calculadora corregida V3"; // Permite comprobar que se abrio esta version.
        }

        // Se utiliza desde los botones del 0 al 9.
        private void Numero(int digito)
        {
            if (error) return;
            if (fin) Limpiar();

            // La conversion necesita solamente N1; se ejecuta al pulsar '='.
            if (operacion == "H" || operacion == "B" || operacion == "O") return;

            if (op)
            {
                // Al comenzar N2, no se reutiliza el numero anterior de la pantalla.
                if (!N2Ingresado || textBox1.Text == "0")
                    textBox1.Text = digito.ToString();
                else
                    textBox1.Text += digito.ToString();

                N2 = Convert.ToDouble(textBox1.Text);
                N2Ingresado = true;

                // Regla solicitada: detectar inmediatamente el divisor cero.
                if (operacion == "/" && N2 == 0)
                {
                    textBox1.Text = "Error: Division por cero";
                    error = true;
                }
            }
            else
            {
                if (textBox1.Text == "0")
                    textBox1.Text = digito.ToString();
                else
                    textBox1.Text += digito.ToString();

                N1 = Convert.ToDouble(textBox1.Text);
            }
        }

        private void Limpiar()
        {
            N1 = 0;
            N2 = 0;
            resultado = 0;
            op = false;
            operacion = "";
            N2Ingresado = false;
            error = false;
            fin = false;
            textBox1.Text = "0";
        }

        private void button1_Click(object sender, EventArgs e) { Numero(1); }
        private void button2_Click(object sender, EventArgs e) { Numero(2); }
        private void button3_Click(object sender, EventArgs e) { Numero(3); }
        private void button4_Click(object sender, EventArgs e) { Numero(4); }
        private void button5_Click(object sender, EventArgs e) { Numero(5); }
        private void button6_Click(object sender, EventArgs e) { Numero(6); }
        private void button7_Click(object sender, EventArgs e) { Numero(7); }
        private void button8_Click(object sender, EventArgs e) { Numero(8); }
        private void button9_Click(object sender, EventArgs e) { Numero(9); }
        private void button11_Click(object sender, EventArgs e) { Numero(0); }

        private void button10_Click(object sender, EventArgs e)
        {
            // El boton decimal queda pendiente; las conversiones requieren enteros.
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (error || op) return;
            operacion = "+";
            op = true;
            fin = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (error || op) return;
            operacion = "-";
            op = true;
            fin = false;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (error || op) return;
            operacion = "*";
            op = true;
            fin = false;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (error || op) return;

            // Regla particular solicitada: rechazar N1 = 0.
            // En matematicas, 0 / 5 seria una division valida.
            if (N1 == 0)
            {
                textBox1.Text = "Error: Division por cero";
                error = true;
                return;
            }

            operacion = "/";
            op = true;
            fin = false;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (error || op) return;
            operacion = "H";
            op = true;
            fin = false;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (error || op) return;
            operacion = "B";
            op = true;
            fin = false;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (error || op) return;
            operacion = "O";
            op = true;
            fin = false;
        }

        // Boton =: resuelve tanto operaciones como conversiones.
        private void button16_Click(object sender, EventArgs e)
        {
            if (error || operacion == "") return;

            // Las conversiones NO deben terminar mostrando resultado.ToString().
            if (operacion == "H" || operacion == "B" || operacion == "O")
            {
                if (N1 != Math.Truncate(N1) || N1 < int.MinValue || N1 > int.MaxValue)
                {
                    textBox1.Text = "Error: Se requiere un entero de 32 bits";
                    error = true;
                    return;
                }

                int b = 2;
                if (operacion == "H") b = 16;
                if (operacion == "O") b = 8;

                // Evita mostrar complemento a dos para numeros negativos.
                long numero = (long)N1;
                string convertido = Convert.ToString(Math.Abs(numero), b).ToUpperInvariant();
                if (numero < 0) convertido = "-" + convertido;

                textBox1.Text = convertido;
            }
            else
            {
                // No calcular si todavia no se ha introducido N2.
                if (!N2Ingresado) return;

                switch (operacion)
                {
                    case "+":
                        resultado = N1 + N2;
                        break;
                    case "-":
                        resultado = N1 - N2;
                        break;
                    case "*":
                        resultado = N1 * N2;
                        break;
                    case "/":
                        if (N1 == 0 || N2 == 0)
                        {
                            textBox1.Text = "Error: Division por cero";
                            error = true;
                            return;
                        }
                        resultado = N1 / N2;
                        break;
                }

                N1 = resultado;
                textBox1.Text = resultado.ToString();
            }

            op = false;
            operacion = "";
            N2 = 0;
            N2Ingresado = false;
            fin = true;
        }

        // Boton <-: elimina el ULTIMO caracter, no obtiene el residuo.
        private void button12_Click(object sender, EventArgs e)
        {
            if (error) return;

            // Tras '=' se permite retroceder visualmente en el resultado;
            // el siguiente digito inicia una operacion nueva.
            if (fin)
            {
                if (textBox1.Text.Length > 1)
                    textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                else
                    textBox1.Text = "0";
                return;
            }

            // Si se selecciono un operador y todavia no se escribio N2,
            // no modificar N1 por accidente.
            if (op && operacion != "H" && operacion != "B" && operacion != "O" && !N2Ingresado)
                return;

            if (textBox1.Text.Length > 1)
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
            else
                textBox1.Text = "0";

            if (op && operacion != "H" && operacion != "B" && operacion != "O")
            {
                N2 = Convert.ToDouble(textBox1.Text);
                N2Ingresado = textBox1.Text != "0";
            }
            else
            {
                N1 = Convert.ToDouble(textBox1.Text);
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
