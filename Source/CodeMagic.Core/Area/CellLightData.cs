using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public class CellLightData : IEnumerable<Light>
    {
        private readonly List<Light> data;

        public CellLightData()
        {
            data = new List<Light>();
        }

        public bool IsBlinding => data.Any(light => light.Power == LightLevel.Blinding);

        public void AddLight(Light light)
        {
            var oldLight = data.FirstOrDefault(l => l.Equals(light));
            if (oldLight != null)
            {
                if (oldLight.Power < light.Power)
                {
                    oldLight.Power = light.Power;
                }
                return;
            }

            data.Add(light.Clone());
        }

        public void AddLights(IEnumerable<Light> lights)
        {
            foreach (var light in lights)
            {
                AddLight(light);
            }
        }

        public bool Contains(Light light)
        {
            return data.Any(instance => instance.Equals(light));
        }

        public void Clear()
        {
            data.Clear();
        }

        public int Count => data.Count;

        public LightLevel GetMaxLightLevel()
        {
            if (data.Count == 0)
                return LightLevel.Darkness;

            return data.Max(light => light.Power);
        }

        public IEnumerator<Light> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Light
    {
        private readonly ILightSource source;

        public Light(ILightSource source)
        {
            Color = source.LightColor;
            Power = source.LightPower;
            this.source = source;
        }

        public Light(Color color, LightLevel power, ILightSource source)
        {
            Color = color;
            Power = power;
            this.source = source;
        }

        public Color Color { get; }

        public LightLevel Power { get; set; }

        public Light Clone(LightLevel power)
        {
            return new Light(Color, power, source);
        }

        public Light Clone()
        {
            return new Light(Color, Power, source);
        }

        public bool Equals(Light other)
        {
            return ReferenceEquals(source, other.source);
        }
    }
}