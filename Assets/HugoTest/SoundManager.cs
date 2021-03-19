using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> nameList = new List<AudioClip>(); //Joueur, Le village, La voyante, Les Loups Garou, Le Chasseur, La sorci�re, Un simple villageois

    public List<AudioClip> actionList = new List<AudioClip>(); //se r�veille, se r�veillent, se rendort, se rendorment, s'endort

    public List<AudioClip> conjonctionList = new List<AudioClip>(); //qui �tait, Et

    public List<AudioClip> instructionList = new List<AudioClip>(); //Instruction 1....10
    
    public List<AudioClip> narrationList = new List<AudioClip>(); //Narration 1....10

    public AudioSource audioSource;

    private void Start()
    {
        /***Exemple***/

        //PlayNarration(0); //Intro
        PlayInstruction(0); //Levez un bras au-dessus de votre t�te pour indiquer que vous �tes pr�ts
        PlayNarration(1); //C�est la nuit, tout le village s�endort
        PlayInstruction(1); //Placez votre masque sur vos yeux
        PlayInstruction(2); //Vous allez � pr�sent d�couvrir tout � tour vos r�les

        PlayerAndAction(1, 0); //Joueur 1 - se r�veille
        PlayInstruction(3); //voici ton r�le, appuie dessus pour le valider
        PlayerAndAction(1, 1); //Joueur 1 - se rendors

        PlayNarration(3); //R�gle du jeu
        PlayNarration(2); //C�est le matin, tout le village se r�veille
        PlayInstruction(4); //Vous pouvez relever vos masques
        PlayNarration(4); //A pr�sent vous devez voter et �lire votre capitaine.
        PlayInstruction(6); //Le capitaine tranchera en cas d��galit� lors d�un vote. Choisissez-le bien ! Utilisez le syst�me de vote pour �lire votre capitaine.
        PlayNarration(1); //C�est la nuit, tout le village s�endort
        PlayInstruction(1); //Placez votre masque sur vos yeux

        PlayerAndAction(9, 0); //Joueur 1 - se r�veille
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
}
