using System.Globalization;

namespace appCalificaciones;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        dpFecha.Date = DateTime.Today;
    }

   
    private bool TryLeerNota(Entry entry, string nombreCampo, out double valor)
    {
        valor = 0;
        string texto = entry.Text?.Trim().Replace(',', '.') ?? "";

        if (string.IsNullOrEmpty(texto))
        {
            MostrarError($"Ingrese el campo: {nombreCampo}.", entry);
            return false;
        }

        if (!double.TryParse(texto, NumberStyles.Float, CultureInfo.InvariantCulture, out valor))
        {
            MostrarError($"{nombreCampo} debe ser un valor numérico.", entry);
            return false;
        }

        if (valor < 0 || valor > 10)
        {
            MostrarError($"{nombreCampo} debe estar entre 0 y 10.", entry);
            return false;
        }

        return true;
    }

    private async void MostrarError(string mensaje, Entry entry)
    {
        await DisplayAlert("Error de validación", mensaje, "Aceptar");
        entry.Focus();
    }

    private async void OnCalcularClicked(object sender, EventArgs e)
    {
        
        if (pckEstudiante.SelectedIndex == -1)
        {
            await DisplayAlert("Error de validación", "Seleccione un estudiante.", "Aceptar");
            return;
        }

        if (!TryLeerNota(txtSeguimiento1, "Nota 1", out double seg1)) return;
        if (!TryLeerNota(txtExamen1, "Examen 1", out double exa1)) return;
        if (!TryLeerNota(txtSeguimiento2, "Nota  2", out double seg2)) return;
        if (!TryLeerNota(txtExamen2, "Examen 2", out double exa2)) return;

    
        double parcial1 = (seg1 * 0.3) + (exa1 * 0.2);
        double parcial2 = (seg2 * 0.3) + (exa2 * 0.2);
        double notaFinal = parcial1 + parcial2;

 
        string estado;
        if (notaFinal >= 7)
            estado = "APROBADO";
        else if (notaFinal >= 5 && notaFinal <= 6.9)
            estado = "COMPLEMENTARIO";
        else
            estado = "REPROBADO";

        lblParcial1.Text = $"Nota Parcial 1: {parcial1:F2}";
        lblParcial2.Text = $"Nota Parcial 2: {parcial2:F2}";
        lblNotaFinal.Text = $"Nota Final: {notaFinal:F2}";
        lblEstado.Text = $"Estado: {estado}";

    
        string mensaje =
            $"Nombre: {pckEstudiante.SelectedItem}\n" +
            $"Fecha: {dpFecha.Date:dd/MM/yyyy}\n" +
            $"Nota Parcial 1: {parcial1:F2}\n" +
            $"Nota Parcial 2: {parcial2:F2}\n" +
            $"Nota Final: {notaFinal:F2}\n" +
            $"Estado: {estado}";

        await DisplayAlert("Resultado", mensaje, "Aceptar");
    }

    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        pckEstudiante.SelectedIndex = -1;
        dpFecha.Date = DateTime.Today;
        txtSeguimiento1.Text = txtExamen1.Text = string.Empty;
        txtSeguimiento2.Text = txtExamen2.Text = string.Empty;
        lblParcial1.Text = "Nota Parcial 1: -";
        lblParcial2.Text = "Nota Parcial 2: -";
        lblNotaFinal.Text = "Nota Final: -";
        lblEstado.Text = "Estado: -";
    }
}
