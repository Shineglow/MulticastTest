using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class CharacterStatTableView : MonoBehaviour
{
    [SerializeField] private RectTransform names;
    [SerializeField] private RectTransform values;
    
    [SerializeField] private TextMeshProUGUI textPrefab;

    private Dictionary<string, (TextMeshProUGUI name, TextMeshProUGUI value)> nameToViews;
    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Init(List<(string name, IReadOnlyReactiveProperty<float> value)> data)
    {
        gameObject.SetActive(true);

        nameToViews = new(data.Count);
        
        foreach ((string name, IReadOnlyReactiveProperty<float> value) in data)
        {
            var nameTMP = Instantiate(textPrefab, names.transform);
            var valueTMP = Instantiate(textPrefab, values.transform);
            nameTMP.text = name;
            valueTMP.text = value.Value.ToString("F2");

            nameTMP.alignment = TextAlignmentOptions.Left;
            valueTMP.alignment = TextAlignmentOptions.Right;
            
            nameToViews.Add(name, (nameTMP, valueTMP));

            value.Subscribe(f => valueTMP.text = f.ToString("F2")).
                AddTo(_disposables);
        }
    }
    
    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
