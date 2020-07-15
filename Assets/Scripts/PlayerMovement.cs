using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private BattleFinder m_battleFinder;
    private SpriteRenderer m_spriteRenderer;

    [SerializeField] private float moveSpeed;

    private bool freezePlayer = false;
    

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_battleFinder = GetComponent<BattleFinder>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (freezePlayer) return;
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        m_animator.SetFloat("xMovement", movementInput.x);
        m_animator.SetFloat("yMovement", movementInput.y);

        m_rigidbody.velocity = movementInput.normalized * moveSpeed;

        if (movementInput.magnitude > 0) m_battleFinder.UpdateStepCounter();

    }

    public void FreezePlayer(bool hideSprite = false)
    {
        freezePlayer = true;
        m_rigidbody.velocity = Vector2.zero;
        m_animator.SetFloat("xMovement", 0.0f);
        m_animator.SetFloat("yMovement", 0.0f);
        m_spriteRenderer.enabled = !hideSprite;
    }

    public void UnFreezePlayer()
    {
        freezePlayer = false;
        m_spriteRenderer.enabled = true;
    }
}
