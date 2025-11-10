using System;
using System.Collections.Generic;

// Ejercicio 1: Abstracción, Encapsulación y Validación
abstract class Envio
{
    public string Descripcion { get; set; }

    private double _peso;

    // Propiedad con validación: no permite valores negativos
    public double Peso
    {
        get { return _peso; }
        set
        {
            if (value >= 0)
            {
                _peso = value;
            }
            else
            {
                _peso = 0.0;
            }
        }
    }

    // Propiedad de solo lectura
    public double CostoPorKg
    {
        get { return 2.0; }
    }

    // Constructor
    public Envio(string descripcion, double peso)
    {
        Descripcion = descripcion;
        Peso = peso;
    }

    // Método abstracto polimórfico
    public abstract double CalcularCostoTotal();

    // Método virtual
    public override string ToString()
    {
        return $"Envío: {Descripcion} | Peso: {Peso:F2} kg | Costo base por kg: {CostoPorKg:C2}";
    }
}

// Ejercicio 2: Herencia y Primera Implementación (PaqueteEstandar)
class PaqueteEstandar : Envio
{
    private double _tarifaPlana;

    public double TarifaPlana
    {
        get { return _tarifaPlana; }
        set
        {
            if (value >= 0)
            {
                _tarifaPlana = value;
            }
            else
            {
                _tarifaPlana = 0.0;
            }
        }
    }

    // Constructor
    public PaqueteEstandar(string descripcion, double peso, double tarifaPlana)
        : base(descripcion, peso)
    {
        TarifaPlana = tarifaPlana;
    }

    // Implementación del método polimórfico
    public override double CalcularCostoTotal()
    {
        double costoBase = Peso * CostoPorKg;
        return costoBase + TarifaPlana;
    }

    public override string ToString()
    {
        return $"[Paquete Estándar] {base.ToString()} | Tarifa plana: {TarifaPlana:C2} | Costo total: {CalcularCostoTotal():C2}";
    }
}

// Ejercicio 3: Atributos Específicos y Polimorfismo Final (PaqueteExpress)
class PaqueteExpress : Envio
{
    private double _recargoUrgencia;

    public double RecargoUrgencia
    {
        get { return _recargoUrgencia; }
        set
        {
            if (value >= 0)
            {
                _recargoUrgencia = value;
            }
            else
            {
                _recargoUrgencia = 0.0;
            }
        }
    }

    // Constructor
    public PaqueteExpress(string descripcion, double peso, double recargoUrgencia)
        : base(descripcion, peso)
    {
        RecargoUrgencia = recargoUrgencia;
    }

    // Implementación del método polimórfico
    public override double CalcularCostoTotal()
    {
        double costoBase = Peso * CostoPorKg;
        double costoRecargo = RecargoUrgencia * Peso;
        return costoBase + costoRecargo;
    }

    public override string ToString()
    {
        return $"[Paquete Express] {base.ToString()} | Recargo urgencia: {RecargoUrgencia:C2}/kg | Costo total: {CalcularCostoTotal():C2}";
    }
}

// Ejercicio 4: Integración del Sistema de Consola (Polimorfismo con Colecciones)
class Program
{
    static List<Envio> envios = new List<Envio>();

    static void Main()
    {
        int opcion;

        do
        {
            MostrarMenu();
            opcion = LeerOpcion();

            switch (opcion)
            {
                case 1:
                    CrearEnvio();
                    break;
                case 2:
                    VerCostosIndividuales();
                    break;
                case 3:
                    CalcularIngresoTotal();
                    break;
                case 4:
                    Console.WriteLine("Saliendo del sistema...");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

        } while (opcion != 4);
    }

    // Mostrar menú
    static void MostrarMenu()
    {
        Console.WriteLine("\n========== LogiTrack - Sistema de Gestión de Envíos ==========");
        Console.WriteLine("1. Crear Envío");
        Console.WriteLine("2. Ver Costos Individuales");
        Console.WriteLine("3. Calcular Ingreso Total");
        Console.WriteLine("4. Salir");
    }

    // Leer opción del usuario
    static int LeerOpcion()
    {
        while (true)
        {
            Console.Write("Seleccione una opción (1-4): ");
            int numero;
            bool esNumero = int.TryParse(Console.ReadLine(), out numero);
            if (esNumero && numero >= 1 && numero <= 4)
            {
                return numero;
            }
            Console.WriteLine("Entrada inválida. Intente nuevamente.");
        }
    }

    // Crear nuevo envío
    static void CrearEnvio()
    {
        Console.WriteLine("\nSeleccione el tipo de envío:");
        Console.WriteLine("1. Paquete Estándar");
        Console.WriteLine("2. Paquete Express");

        int tipo = 0;
        while (true)
        {
            Console.Write("Opción (1-2): ");
            bool esNumero = int.TryParse(Console.ReadLine(), out tipo);
            if (esNumero && tipo >= 1 && tipo <= 2)
            {
                break;
            }
            Console.WriteLine("Entrada inválida. Intente nuevamente.");
        }

        Console.Write("Descripción del envío: ");
        string descripcion = Console.ReadLine();

        double peso = LeerDoublePositivo("Peso (kg): ");

        if (tipo == 1)
        {
            double tarifa = LeerDoublePositivo("Tarifa plana (€): ");
            envios.Add(new PaqueteEstandar(descripcion, peso, tarifa));
            Console.WriteLine("✅ Paquete estándar creado correctamente.");
        }
        else if (tipo == 2)
        {
            double recargo = LeerDoublePositivo("Recargo por urgencia (€/kg): ");
            envios.Add(new PaqueteExpress(descripcion, peso, recargo));
            Console.WriteLine("✅ Paquete express creado correctamente.");
        }
    }

    // Leer valor positivo
    static double LeerDoublePositivo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            double valor;
            bool esNumero = double.TryParse(Console.ReadLine(), out valor);
            if (esNumero && valor >= 0)
            {
                return valor;
            }
            Console.WriteLine("Valor inválido. Debe ser un número no negativo.");
        }
    }

    // Ver costos individuales
    static void VerCostosIndividuales()
    {
        Console.WriteLine("\n--- Costos Individuales ---");
        if (envios.Count == 0)
        {
            Console.WriteLine("No hay envíos registrados.");
            return;
        }

        int i = 1;
        foreach (Envio e in envios)
        {
            Console.WriteLine($"{i}. {e}");
            i++;
        }
    }

    // Calcular ingreso total
    static void CalcularIngresoTotal()
    {
        double total = 0;
        foreach (Envio e in envios)
        {
            total += e.CalcularCostoTotal();
        }

        Console.WriteLine($"\nIngreso total por envíos: {total:C2}");
    }
}
