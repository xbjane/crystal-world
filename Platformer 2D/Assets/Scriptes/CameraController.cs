using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;//для записи координаты движения

    private void Awake()//поиск игрока
    {
        if (!player)//проверка найден ли игрок
            player = FindObjectOfType<Hero>().transform;
    }
    //private void Start()
    //{
    //    pos = player.position;//получаем координаты игрока
    //    pos.z = -10f;//фиксируем положение камеры, чтобы она не приближалась к игроку
    //    transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);//перемещаем туда камеру, метод Lerp делает движение плавным

    //}
    private void Update()
    {
        pos = player.position;//получаем координаты игрока
        pos.z = -10f;//фиксируем положение камеры, чтобы она не приближалась к игроку
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);//перемещаем туда камеру, метод Lerp делает движение плавным
    }
}
