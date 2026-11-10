using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EnvioCliente.controller;
using EnvioCliente.observer;

namespace EnvioCliente.view
{
    public partial class Form1 : Form
    {
        // Ventanas abiertas (una por caso de uso). Son no modales: se pueden tener varias a la vez.
        private readonly Dictionary<Type, Form> ventanasAbiertas = new Dictionary<Type, Form>();

        private ToolStripMenuItem graphQlMenuItem;
        private ToolStripMenuItem restMenuItem;

        public Form1()
        {
            InitializeComponent();
            envioNacionalToolStripMenuItem.DropDownItems.Add("Envios con paquetes", null, (s, e) => abrir<GUIEnviosConPaquetes>());
            construirMenuPaquete();
            construirMenuServicio();
            mostrarProtocolo();
        }

        // Menu "Paquete": casos de uso del detalle (cada opcion abre una ventana con un solo caso de uso).
        private void construirMenuPaquete()
        {
            var paqueteMenu = new ToolStripMenuItem("Paquete");
            paqueteMenu.DropDownItems.Add("Adicionar", null, (s, e) => abrir<GUIAdicionarPaquete>());
            paqueteMenu.DropDownItems.Add("Consultar", null, (s, e) => abrir<GUIConsultarPaquete>());
            paqueteMenu.DropDownItems.Add("Listar", null, (s, e) => abrir<GUIListarPaquete>());
            paqueteMenu.DropDownItems.Add("Actualizar", null, (s, e) => abrir<GUIActualizarPaquete>());
            paqueteMenu.DropDownItems.Add("Eliminar", null, (s, e) => abrir<GUIEliminarPaquete>());

            menuStrip1.Items.Insert(menuStrip1.Items.IndexOf(envioNacionalToolStripMenuItem) + 1, paqueteMenu);
        }

        // Abre la ventana del caso de uso sin bloquear las demas. Si ya esta abierta, la trae al frente.
        private void abrir<T>() where T : Form, new()
        {
            Form ventana;
            if (ventanasAbiertas.TryGetValue(typeof(T), out ventana) && !ventana.IsDisposed)
            {
                if (ventana.WindowState == FormWindowState.Minimized)
                {
                    ventana.WindowState = FormWindowState.Normal;
                }
                ventana.Activate();
                return;
            }

            ventana = new T();
            ventanasAbiertas[typeof(T)] = ventana;

            // En cascada, para que las ventanas no queden una encima de otra.
            int n = ventanasAbiertas.Values.Count(v => !v.IsDisposed) - 1;
            ventana.StartPosition = FormStartPosition.Manual;
            ventana.Location = new Point(Location.X + 40 + 30 * (n % 8), Location.Y + 60 + 30 * (n % 8));
            ventana.Show(this);
        }

        // Menu "Servicio web": permite elegir si el cliente consume GraphQL o REST.
        private void construirMenuServicio()
        {
            graphQlMenuItem = new ToolStripMenuItem("GraphQL");
            restMenuItem = new ToolStripMenuItem("REST");

            graphQlMenuItem.Click += (s, e) => cambiarProtocolo(true);
            restMenuItem.Click += (s, e) => cambiarProtocolo(false);

            var servicioMenu = new ToolStripMenuItem("Servicio web");
            servicioMenu.DropDownItems.Add(graphQlMenuItem);
            servicioMenu.DropDownItems.Add(restMenuItem);

            menuStrip1.Items.Insert(menuStrip1.Items.IndexOf(ayudaToolStripMenuItem), servicioMenu);
        }

        private void cambiarProtocolo(bool graphQl)
        {
            if (graphQl)
            {
                EnvioNacionalController.Instance.UsarGraphQL();
            }
            else
            {
                EnvioNacionalController.Instance.UsarRest();
            }

            mostrarProtocolo();

            // Refresca la ventana Listar (si esta abierta) usando el nuevo protocolo.
            EnviosSubject.Instance.Notificar();
        }

        private void mostrarProtocolo()
        {
            bool graphQl = EnvioNacionalController.Instance.Protocolo == "GraphQL";
            graphQlMenuItem.Checked = graphQl;
            restMenuItem.Checked = !graphQl;
            Text = "Gestión de Envíos Nacionales - " + EnvioNacionalController.Instance.Protocolo;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void adicionarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir<GUIAdicionarEnvioNacional>();
        }

        private void consultarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir<GUIConsultarEnvioNacional>();
        }

        private void listarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir<GUIListarEnvioNacional>();
        }

        private void actualizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir<GUIActualizarEnvioNacional>();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir<GUIEliminarEnvioNacional>();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Gestión de Envíos Nacionales\nVersión 1.0\n\nIntegrantes:\n" +
                "Juan Esteban Molano Polanco codigo 2220232008\n" +
                "Juan Pablo Chacon codigo 2220232030\n" +
                "Johan Daniel Esterling codigo 2220241033",
                "Acerca de...");
        }
    }
}
