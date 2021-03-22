using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> nameList = new List<AudioClip>(); //Joueur, Le village, La voyante, Les Loups Garou, Le Chasseur, La sorcière, Un simple villageois
    [SerializeField]
    private List<AudioClip> actionList = new List<AudioClip>(); //se réveille, se réveillent, se rendort, se rendorment, s'endort
    [SerializeField]
    private List<AudioClip> conjonctionList = new List<AudioClip>(); //qui était, Et
    [SerializeField]
    private List<AudioClip> instructionList = new List<AudioClip>(); //Instruction 1....10
    [SerializeField]
    private List<AudioClip> narrationList = new List<AudioClip>(); //Narration 1....10
    [SerializeField]
    private List<AudioClip> sfxList = new List<AudioClip>(); //GameBegin, Jour se leve, Nuit tombe, Vote Begin, Vote End, Voyante reveal, LG mange, Witch popo, Chasseur tire, Village win, LG win

    [SerializeField]
    private AudioClip ambientDay, ambientNight;
    [SerializeField]
    private AudioSource audioSource, ambientSource;

    private bool villageAwake = true;

    private void Start()
    {
        ambientSource.loop = true;
        /***Exemple***/

        //PlayNarration(0); //Intro
        PlayInstruction(0); //Levez un bras au-dessus de votre tête pour indiquer que vous êtes prêts
        PlayNarration(1); //C’est la nuit, tout le village s’endort
        PlayInstruction(1); //Placez votre masque sur vos yeux
        PlayInstruction(2); //Vous allez à présent découvrir tout à tour vos rôles

        PlayerAndAction(1, 0); //Joueur 1 - se réveille
        PlayInstruction(3); //voici ton rôle, appuie dessus pour le valider
        PlayerAndAction(1, 1); //Joueur 1 - se rendors

        PlayNarration(3); //Règle du jeu
        PlayNarration(2); //C’est le matin, tout le village se réveille
        PlayInstruction(4); //Vous pouvez relever vos masques
        PlayNarration(4); //A présent vous devez voter et élire votre capitaine.
        PlayInstruction(6); //Le capitaine tranchera en cas d’égalité lors d’un vote. Choisissez-le bien ! Utilisez le système de vote pour élire votre capitaine.
        PlayNarration(1); //C’est la nuit, tout le village s’endort
        PlayInstruction(1); //Placez votre masque sur vos yeux

        PlayerAndAction(9, 0); //Joueur 1 - se réveille
        PlayInstruction(6); //Instruction voyante
        PlayerAndAction(9, 1); //Joueur 1 - se rendors
    }

    private AudioClip Combine(params AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        int length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            length += clips[i].samples * clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            float[] buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            //System.Buffer.BlockCopy(buffer, 0, data, length, buffer.Length);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;

        //AudioClip result = AudioClip.Create("Combine", length / 2, 2, 44100, false, false);
        AudioClip result = AudioClip.Create("Combined", length / 2, 2, 44100, false);
        result.SetData(data, 0);

        return result;
    }

    public void SwitchAmbientTime()
    {
        ambientSource.Stop();
        if (villageAwake)
        {
            PlaySFX(1);
            ambientSource.clip = ambientDay;
            villageAwake = false;
        }
        else
        {
            PlaySFX(2);
            ambientSource.clip = ambientNight;
            villageAwake = true;
        }
        ambientSource.Play();
    }

    public void PlaySFX(int id)
    {
        StartCoroutine(PlayClipASAP(Combine(sfxList[id])));
    }

    public void PlayInstruction(int id)
    {
        StartCoroutine(PlayClipASAP(Combine(instructionList[id])));
    }

    public void PlayNarration(int id)
    {
        StartCoroutine(PlayClipASAP(Combine(narrationList[id])));
    }

    public void PlayerAndAction(int playerId, int actionId)
    {
        StartCoroutine(PlayClipASAP(Combine(nameList[playerId], actionList[actionId])));
    }

    public void PlayNightReport()
    {
        //Nobody died last night
        StartCoroutine(PlayClipASAP(Combine(instructionList[8])));
    }

    public void PlayNightReport(int playerId, int role)
    {
        //Someone died last night
        StartCoroutine(PlayClipASAP(Combine(instructionList[9], nameList[playerId], conjonctionList[0] , nameList[role])));
    }

    public void PlayNightReport(int playerId, int role, int playerId2, int role2)
    {
        //Two persons died last night
        StartCoroutine(PlayClipASAP(Combine(instructionList[10], nameList[playerId], conjonctionList[0], nameList[role], conjonctionList[1], nameList[playerId2], conjonctionList[0], nameList[role2])));
    }

    public void PlayVillageVotedFor(int playerId, int role)
    {
        //After villagers voted designed who is supposed to die
        PlayClipASAP(Combine(instructionList[12], nameList[playerId], conjonctionList[0], nameList[role]));
    }

    IEnumerator PlayClipASAP(AudioClip nextClip)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        //audioSource.clip = nextClip;
        audioSource.PlayOneShot(nextClip);
    }

    /*Continuous*/
    //Son d'ambiance Jour / Nuit V
    //Musique Jour / Nuit (OPTIONEL)

    /*One Shot*/
    //Son tombé de la nuit, son levé du jour V

    //Son loup-garou mange V
    //Son chasseur tire V
    //Son voyante reveal V
    //Son sorcière use potion 
    //Son début du vote V
    //Son fin de vote V

    //Musique Village Win V
    //Musique LG Win V

}
